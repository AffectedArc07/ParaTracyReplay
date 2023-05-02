using Serilog;
using ParaTracyReplay.Structures;
using System.Text;
using System.Net;
using System.Net.Sockets;
using ParaTracyReplay.Structures.Network;
using K4os.Compression.LZ4;
using ParaTracyReplay.Structures.File;
using System.Buffers;
using System.Buffers.Binary;

namespace ParaTracyReplay {
    /// <summary>
    /// The actual loader implementation.
    /// This loads the file up and sends it over the network to a Tracy capture client.
    /// </summary>
    internal class Loader {
        /// <summary>
        /// Current timestamp we are working with.
        /// This is class-scoped as its uesd by <see cref="WriteThreadContext(uint)"/> and by <see cref="Load(string)"/>.
        /// </summary>
        private long Timestamp;

        /// <summary>
        /// Current thread ID we are working with.
        /// This is class-scoped as its uesd by <see cref="WriteThreadContext(uint)"/> and by <see cref="Load(string)"/>.
        /// </summary>
        private uint ThreadId;

        /// <summary>
        /// Holding <see cref="byte"/> array to hold data from <see cref="WriteMessage(byte[])"/> before sending as one bunched message in <see cref="Commit"/>.
        /// </summary>
        private byte[] WriteBuffer;

        /// <summary>
        /// The current offset for the <see cref="WriteBuffer"/> array.
        /// </summary>
        private int Offset;

        /// <summary>
        /// The <see cref="Socket"/> representing the connection from the client.
        /// </summary>
        private Socket ClientSocket;

        /// <summary>
        /// Loads the Tracy file from disk and sends it over the network.
        /// </summary>
        /// <param name="file_to_load">The <see cref="string"/> path to the file we want to load.</param>
        public void Load(string file_to_load) {
            // Make sure it exists first
            if (!File.Exists(file_to_load)) {
                Log.Logger.Fatal($"File \"{file_to_load}\" not found!");
                Environment.Exit(1);
            }

            Log.Logger.Information($"Loading \"{file_to_load}\"...");

            // Read it as a file stream not just as the full thing at once
            using (FileStream fs = File.OpenRead(file_to_load)) {
                using (BinaryReader br = new BinaryReader(fs)) {
                    // Get the file header first
                    FileHeader file_header = new FileHeader();
                    file_header.Read(br);

                    // So we can validate its signature
                    if (file_header.Signature == Constants.FileSignature) {
                        Log.Logger.Information("File signature matches");
                    } else {
                        Log.Logger.Fatal($"File signature mismatch! Expected \"{Constants.FileSignature}\", got \"{file_header.Signature}\"");
                        Environment.Exit(1);
                    }

                    // And version
                    if (file_header.Version == Constants.FileVersion) {
                        Log.Logger.Information("File version matches");
                    } else {
                        Log.Logger.Fatal($"File version mismatch! Expected \"{Constants.FileVersion}\", got \"{file_header.Version}\"");
                        Environment.Exit(1);
                    }

                    // Inform of the process name
                    Log.Logger.Information($"Captured process: {string.Join("", file_header.ProgramName)}");

                    // Get how many source locations we have
                    uint source_locations_count = br.ReadUInt32();
                    Log.Logger.Information($"Found {source_locations_count} source locations");

                    // Save our cache of them
                    Dictionary<long, string> strings = new Dictionary<long, string>();
                    Dictionary<ulong, SourceLocation> source_locations = new Dictionary<ulong, SourceLocation>();

                    // Read them all in
                    for (ulong i = 0; i < source_locations_count; i++) {
                        // Track the location name
                        uint loc_name_length = br.ReadUInt32();
                        string loc_name = string.Join("", br.ReadChars((int)loc_name_length));

                        // And the function name
                        uint function_name_len = br.ReadUInt32();
                        string function_name = string.Join("", br.ReadChars((int)function_name_len));

                        // And the file name
                        uint file_name_len = br.ReadUInt32();
                        string file_name = string.Join("", br.ReadChars((int)file_name_len));

                        // And the file name
                        uint line = br.ReadUInt32();
                        uint colour = br.ReadUInt32();

                        // Get the int of the name
                        long name_int = 0;
                        if (!string.IsNullOrWhiteSpace(loc_name)) {
                            name_int = loc_name.GetHashCode();
                            strings[name_int] = loc_name;
                        }

                        // And of the function
                        long function_int = 0;
                        if (!string.IsNullOrWhiteSpace(function_name)) {
                            function_int = function_name.GetHashCode();
                            strings[function_int] = function_name;
                        }

                        // And of the file
                        long file_int = 0;
                        if (!string.IsNullOrWhiteSpace(file_name)) {
                            file_int = file_name.GetHashCode();
                            strings[file_int] = file_name;
                        }

                        // And pack it all into a nice object
                        source_locations[i] = new SourceLocation() {
                            Name = name_int,
                            Function = function_int,
                            File = file_int,
                            Line = line,
                            Colour = colour,
                        };
                    }

                    // Inform of load
                    Log.Logger.Information($"Successfully loaded {source_locations.Count} source locations");

                    // Create our server endpoint
                    IPEndPoint socket_endpoint = new IPEndPoint(IPAddress.Any, 8086);
                    Socket server = new Socket(SocketType.Stream, ProtocolType.Tcp);
                    server.Bind(socket_endpoint);
                    // Allow 1 client max
                    server.Listen(1);

                    // Wait here
                    Log.Logger.Information($"Waiting for connection on {socket_endpoint.Address}:{socket_endpoint.Port}...");

                    // We got a connection
                    ClientSocket = server.Accept();
                    Log.Logger.Information($"Connection established");

                    // Read 8 bytes for the client name
                    byte[] clientname_buffer = new byte[8];
                    ClientSocket.Receive(clientname_buffer);
                    string client_name = Encoding.ASCII.GetString(clientname_buffer);

                    // Validate its a Tracy client
                    if (client_name != "TracyPrf") {
                        Log.Logger.Fatal($"Invalid client (Expected \"TracyPrf\", got \"{client_name}\")");
                        Environment.Exit(1);
                    }

                    // Read 4 bytes for protocol number
                    byte[] protocol_buffer = new byte[4];
                    ClientSocket.Receive(protocol_buffer);
                    uint protocol_version = BitConverter.ToUInt32(protocol_buffer);

                    // We only support these protocol versions
                    uint[] valid_versions = new uint[] { 56, 57 };

                    // Validate protocol version
                    if (!valid_versions.Contains(protocol_version)) {
                        Log.Logger.Fatal($"Invalid protocol version (Got {protocol_version}, valid versions are {string.Join(", ", valid_versions)})");
                        ClientSocket.Send(new byte[] { Constants.NetworkHandshakeProtocolMismatch });
                        Environment.Exit(1);
                    }

                    // If we got here its a valid version, send our welcome
                    ClientSocket.Send(new byte[] { Constants.NetworkHandshakeWelcome });

                    // Make our network header from the file header
                    NetworkHeader net_header = new NetworkHeader();
                    net_header.Multiplier = file_header.Multiplier;
                    net_header.InitBegin = file_header.InitBegin;
                    net_header.InitEnd = file_header.InitEnd;
                    net_header.Delay = file_header.Delay;
                    net_header.Resolution = file_header.Resolution;
                    net_header.Epoch = file_header.Epoch;
                    net_header.ExecTime = file_header.ExecTime;
                    net_header.ProcessId = file_header.ProcessId;
                    net_header.SamplingPeriod = file_header.SamplingPeriod;
                    net_header.Flags = (sbyte)file_header.Flags;
                    net_header.CpuArch = (sbyte)file_header.CpuArch;
                    Array.Copy(file_header.CpuManufacturer, net_header.CpuManufacturer, 12);
                    net_header.CpuId = file_header.CpuId;
                    Array.Copy(file_header.ProgramName, net_header.ProgramName, 64);
                    Array.Copy(file_header.HostInfo, net_header.HostInfo, 64);

                    // Send the network header
                    ClientSocket.Send(net_header.Write());

                    // Setup some vars for the network handling
                    int event_array_size = 24;
                    byte[] event_array = new byte[event_array_size];
                    Timestamp = 0;
                    ThreadId = 0;
                    WriteBuffer = new byte[(int)Math.Floor((decimal)(Constants.NetworkMaxFrameSize / event_array_size))];
                    Offset = 0;

                    Log.Logger.Information("Sending proc events to client...");

                    // Now read file events and send them off
                    // A FileEvent is 24 bytes (maximum), so read into an array of that size
                    while (br.Read(event_array) == event_array_size) {
                        // Make a new event and read the binary in from the file
                        FileEvent file_event = new FileEvent();
                        file_event.Read(new BinaryReader(new MemoryStream(event_array)));

                        // Figure out what event we have
                        switch (file_event.Type) {
                            // Handle ZoneBegin event
                            case Constants.FileEventZoneBegin:
                                FileZoneBegin file_zonebegin = (FileZoneBegin)file_event.Event;
                                WriteThreadContext(file_zonebegin.ThreadId);

                                NetworkZoneBegin network_startevent = new NetworkZoneBegin() {
                                    Type = Constants.NetworkEventZoneBegin,
                                    Timestamp = file_zonebegin.Timestamp - Timestamp,
                                    SourceLocation = file_zonebegin.SourceLocation
                                };

                                WriteMessage(network_startevent.Write());
                                Timestamp = file_zonebegin.Timestamp;
                                break;


                            // Handle ZoneEnd event
                            case Constants.FileEventZoneEnd:
                                FileZoneEnd file_zoneend = (FileZoneEnd)file_event.Event;
                                WriteThreadContext(file_zoneend.ThreadId);

                                NetworkZoneEnd network_endevent = new NetworkZoneEnd() {
                                    Type = Constants.NetworkEventZoneEnd,
                                    Timestamp = file_zoneend.Timestamp - Timestamp
                                };

                                WriteMessage(network_endevent.Write());
                                Timestamp = file_zoneend.Timestamp;
                                break;


                            // Handle ZoneColour event
                            case Constants.FileEventZoneColour:
                                FileZoneColour file_zonecolour = (FileZoneColour)file_event.Event;
                                WriteThreadContext(file_zonecolour.ThreadId);

                                NetworkZoneColour network_colourevent = new NetworkZoneColour() {
                                    Type = Constants.NetworkEventZoneColour,
                                    ColourR = (byte)((file_zonecolour.Colour >> 0x00) & 0xFF),
                                    ColourG = (byte)((file_zonecolour.Colour >> 0x08) & 0xFF),
                                    ColourB = (byte)((file_zonecolour.Colour >> 0x10) & 0xFF)
                                };

                                WriteMessage(network_colourevent.Write());
                                break;


                            // Handle FrameMark event
                            case Constants.FileEventFrameMark:
                                FileFrameMark file_framemarkevent = (FileFrameMark)file_event.Event;

                                NetworkFrameMark network_framemarkevent = new NetworkFrameMark() {
                                    Type = Constants.NetworkEventFrameMark,
                                    Name = 0,
                                    Timestamp = file_framemarkevent.Timestamp
                                };

                                WriteMessage(network_framemarkevent.Write());
                                break;
                        }
                    }

                    // We have read all the proc events, dump to the network and start the next
                    Commit();
                    Log.Logger.Information("Proc events sent. Negotiating string info.");

                    // Network requests are 13 bytes
                    int network_recv_size = 13;
                    byte[] network_recv_buffer = new byte[network_recv_size];

                    // Set this timeout to account for the fact we dont handle the data done packet if that even exists
                    ClientSocket.ReceiveTimeout = 1;
                    try {
                        while (ClientSocket.Receive(network_recv_buffer) == network_recv_size) {
                            // Decode the request
                            NetworkRequest nr = new NetworkRequest();
                            nr.Read(new BinaryReader(new MemoryStream(network_recv_buffer)));
                            // Wipe this, we will need it later
                            Array.Clear(network_recv_buffer);

                            // Figure out what we have
                            switch (nr.Type) {
                                // Source location query, handle it
                                case Constants.NetworkQuerySrcloc:
                                    SourceLocation sourceloc = source_locations[BitConverter.ToUInt64(BitConverter.GetBytes(nr.Pointer))];
                                    NetworkSourceLocation sourceloc_packet = new NetworkSourceLocation() {
                                        Type = Constants.NetworkEventSrcloc,
                                        Name = sourceloc.Name,
                                        Function = sourceloc.Function,
                                        File = sourceloc.File,
                                        Line = sourceloc.Line,
                                        ColourR = (byte)((sourceloc.Colour >> 0x00) & 0xFF),
                                        ColourG = (byte)((sourceloc.Colour >> 0x08) & 0xFF),
                                        ColourB = (byte)((sourceloc.Colour >> 0x10) & 0xFF)
                                    };
                                    WriteMessage(sourceloc_packet.Write());
                                    break;

                                // String query, handle it
                                case Constants.NetworkQueryString:
                                    WriteStringResponse(strings[nr.Pointer].ToCharArray(), BitConverter.ToUInt64(BitConverter.GetBytes(nr.Pointer)), Constants.NetworkResponseStringData);
                                    break;

                                // No symbol code handling
                                case Constants.NetworkQuerySymbolCode:
                                    WriteMessage(new byte[] { Constants.NetworkResponseSymbolCodeNotAvailable });
                                    break;

                                // No source code handling
                                case Constants.NetworkQuerySourceCode:
                                    WriteMessage(new byte[] { Constants.NetworkResponseSourceCodeNotAvailable });
                                    break;

                                // No data transfer handling
                                case Constants.NetworkQueryDataTransfer:
                                    WriteMessage(new byte[] { Constants.NetworkResponseServerQueryNoop });
                                    break;

                                // No partial data transfer handling
                                case Constants.NetworkQueryDataTransferPart:
                                    WriteMessage(new byte[] { Constants.NetworkResponseServerQueryNoop });
                                    break;

                                // Handle thread names. Only one.
                                case Constants.NetworkQueryThreadString:
                                    WriteStringResponse("main".ToCharArray(), BitConverter.ToUInt64(BitConverter.GetBytes(nr.Pointer)), Constants.NetworkResponseThreadName);
                                    break;

                                // Infom on packets we dont recognise
                                default:
                                    Log.Logger.Warning($"Unknown request from Tracy (Packet type: {nr.Type})");
                                    break;
                            }
                        }
                    } catch (SocketException exc) {
                        // Timeouts are expected since we dont parse the "done" message
                        if (exc.SocketErrorCode != SocketError.TimedOut) {
                            // If its not a timeout, kick up a fuss
                            throw exc;
                        }
                    }

                    // Dump it all
                    Commit();
                    Log.Logger.Information("Data transfer complete");

                    // And clean up
                    ClientSocket.Close();
                    server.Close();
                    Log.Logger.Information("Done!");
                }
            }
        }

        /// <summary>
        /// Sends the current <see cref="WriteBuffer"/> down the <see cref="ClientSocket"/>, after encoding it with LZ4.
        /// </summary>
        private void Commit() {
            if (Offset > 0) {
                // Make our buffers
                ArrayBufferWriter<byte> buffer = new ArrayBufferWriter<byte>();
                ArrayBufferWriter<byte> encoded = new ArrayBufferWriter<byte>();

                // Encode it 
                encoded.Advance(LZ4Codec.Encode(WriteBuffer.AsSpan()[..Offset], encoded.GetSpan(LZ4Codec.MaximumOutputSize(Offset))));

                // Get the amount written
                BinaryPrimitives.WriteUInt32LittleEndian(buffer.GetSpan(sizeof(uint)), (uint)encoded.WrittenCount);
                // Advance once
                buffer.Advance(sizeof(uint));
                // Write the encoded data
                buffer.Write(encoded.WrittenSpan);

                // Fire it at Tracy
                ClientSocket.Send(buffer.WrittenSpan);

                // Cleanup
                Array.Clear(WriteBuffer);
                Offset = 0;
            }
        }

        /// <summary>
        /// Adds the message param to the <see cref="WriteBuffer"/>, running <see cref="Commit"/> if required.
        /// </summary>
        /// <param name="message">The <see cref="byte"/> array to write.</param>
        private void WriteMessage(byte[] message) {
            // Flush buffer if required
            if ((Offset + message.Length) > WriteBuffer.Length) {
                Commit();
            }

            // Clone the message to our write buffer and offset it
            Array.Copy(message, 0, WriteBuffer, Offset, message.Length);
            Offset += message.Length;
        }

        /// <summary>
        /// Writes a thread ID to the <see cref="ClientSocket"/>.
        /// </summary>
        /// <param name="passed_threadid">The thread ID to write</param>
        private void WriteThreadContext(uint passed_threadid) {
            // If its different, send a new thread and reset the timestamp
            if (ThreadId != passed_threadid) {
                ThreadId = passed_threadid;
                Timestamp = 0;

                // Make the event
                NetworkThreadContext netthread_event = new NetworkThreadContext() {
                    ThreadId = passed_threadid,
                    Type = Constants.NetworkEventThreadContext
                };

                // And fire
                WriteMessage(netthread_event.Write());
            }
        }

        /// <summary>
        /// Writes a response to a string query to the <see cref="ClientSocket"/>.
        /// </summary>
        /// <param name="str">The string as a <see cref="char"/> array.</param>
        /// <param name="pointer">The pointer expressed as a <see cref="ulong"/>.</param>
        /// <param name="stringtype">The string type expressed as a <see cref="byte"/>.</param>
        private void WriteStringResponse(char[] str, ulong pointer, byte stringtype) {
            // Get the length of the array
            ushort string_length = (ushort)str.Length;

            // Make the event
            NetworkStringData string_packet = new NetworkStringData() {
                Type = stringtype,
                Pointer = pointer,
                StringLength = string_length,
                String = str
            };

            // And fire
            WriteMessage(string_packet.Write());
        }
    }
}
