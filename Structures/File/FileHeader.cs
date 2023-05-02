using System.Text;

namespace ParaTracyReplay.Structures {
    /// <summary>
    /// Represents the header inside of the data file.
    /// </summary>
    internal class FileHeader : StructureBase {
        /// <summary>
        /// The signature of the file expressed as a <see cref="ulong"/>.
        /// </summary>
        public ulong Signature { get; set; }

        /// <summary>
        /// The version of the file, expressed as a <see cref="uint"/>.
        /// </summary>
        public uint Version { get; set; }

        /// <summary>
        /// I dont actually know what this means, but its expressed as a <see cref="double"/>.
        /// </summary>
        public double Multiplier { get; set; }

        /// <summary>
        /// I dont actually know what this means, but its expressed as a <see cref="long"/>.
        /// </summary>
        public long InitBegin { get; set; }

        /// <summary>
        /// I dont actually know what this means, but its expressed as a <see cref="long"/>.
        /// </summary>
        public long InitEnd { get; set; }

        /// <summary>
        /// I dont actually know what this means, but its expressed as a <see cref="long"/>.
        /// </summary>
        public long Delay { get; set; }

        /// <summary>
        /// I dont actually know what this means, but its expressed as a <see cref="long"/>.
        /// </summary>
        public long Resolution { get; set; }

        /// <summary>
        /// I dont actually know what this means, but its expressed as a <see cref="long"/>.
        /// </summary>
        public long Epoch { get; set; }

        /// <summary>
        /// I dont actually know what this means, but its expressed as a <see cref="long"/>.
        /// </summary>
        public long ExecTime { get; set; }

        /// <summary>
        /// The PID of the profiled process, expressed as a <see cref="long"/>.
        /// </summary>
        public long ProcessId { get; set; }

        /// <summary>
        /// I dont actually know what this means, but its expressed as a <see cref="long"/>.
        /// </summary>
        public long SamplingPeriod { get; set; }

        /// <summary>
        /// I dont actually know what this means, but its expressed as a <see cref="byte"/>.
        /// </summary>
        public byte Flags { get; set; }

        /// <summary>
        /// The architecture of the CPU the process ran on, expressed as a <see cref="byte"/>.
        /// </summary>
        public byte CpuArch { get; set; }

        /// <summary>
        /// The manufacturer of the CPU the process ran on, expressed as an ASCII <see cref="char"/> array.
        /// </summary>
        public char[] CpuManufacturer { get; set; }

        /// <summary>
        /// I dont actually know what this means, but its expressed as a <see cref="uint"/>.
        /// </summary>
        public uint CpuId { get; set; }

        /// <summary>
        /// The name of the proccess profiled, expressed as an ASCII <see cref="char"/> array.
        /// </summary>
        public char[] ProgramName { get; set; }

        /// <summary>
        /// I dont actually know what this means, but its expressed as an ASCII <see cref="char"/> array.
        /// </summary>
        public char[] HostInfo { get; set; }

        /// <summary>
        /// Creates a new <see cref="FileHeader"/>.
        /// </summary>
        public FileHeader() {
            // Setup the byte arrays
            CpuManufacturer = new char[12];
            ProgramName = new char[64];
            HostInfo = new char[1024];
        }

        /// <inheritdoc/>
        public override byte[] Write() {
            MemoryStream stream = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
                writer.Write(Signature);
                writer.Write(Version);
                writer.Write(Multiplier);
                writer.Write(InitBegin);
                writer.Write(InitEnd);
                writer.Write(Delay);
                writer.Write(Resolution);
                writer.Write(Epoch);
                writer.Write(ExecTime);
                writer.Write(ProcessId);
                writer.Write(SamplingPeriod);
                writer.Write(Flags);
                writer.Write(CpuArch);
                writer.Write(CpuManufacturer);
                writer.Write(CpuId);
                writer.Write(ProgramName);
                writer.Write(HostInfo);
            }

            return stream.ToArray();
        }

        /// <inheritdoc/>
        public override void Read(BinaryReader reader) {	
            //                                                                                                                                 POS:    0
            Signature = reader.ReadUInt64();           // ("signature", ctypes.c_ulonglong),           Start at 0, read 8 bytes                POS:    8
            Version = reader.ReadUInt32();             // ("version", ctypes.c_uint),                  Start at 8, read 4 bytes                POS:   12
            reader.ReadBytes(4);                       //                                              Skip 4 bytes due to alignment rules     POS:   16
            Multiplier = reader.ReadDouble();          // ("multiplier", ctypes.c_double),             Start at 16, read 8 bytes               POS:   24
            InitBegin = reader.ReadInt64();            // ("init_begin", ctypes.c_longlong),           Start at 24, read 8 bytes               POS:   32
            InitEnd = reader.ReadInt64();              // ("init_end", ctypes.c_longlong),             Start at 32, read 8 bytes               POS:   40
            Delay = reader.ReadInt64();                // ("delay", ctypes.c_longlong),                Start at 40, read 8 bytes               POS:   48
            Resolution = reader.ReadInt64();           // ("resolution", ctypes.c_longlong),           Start at 48, read 8 bytes               POS:   56
            Epoch = reader.ReadInt64();                // ("epoch", ctypes.c_longlong),                Start at 56, read 8 bytes               POS:   64
            ExecTime = reader.ReadInt64();             // ("exec_time", ctypes.c_longlong),            Start at 64, read 8 bytes               POS:   72
            ProcessId = reader.ReadInt64();            // ("pid", ctypes.c_longlong),                  Start at 72, read 8 bytes               POS:   80
            SamplingPeriod = reader.ReadInt64();       // ("sampling_period", ctypes.c_longlong),      Start at 80, read 8 bytes               POS:   88
            Flags = reader.ReadByte();                 // ("flags", ctypes.c_byte),                    Start at 88, read 1 byte                POS:   89
            CpuArch = reader.ReadByte();               // ("cpu_arch", ctypes.c_byte),                 Start at 89, read 1 byte                POS:   90
            CpuManufacturer = reader.ReadChars(12);    // ("cpu_manufacturer", ctypes.c_char * 12),    Start at 90, read 12 bytes              POS:  102
            reader.ReadBytes(2);                       //                                              Skip 2 bytes due to alignment rules     POS:  104
            CpuId = reader.ReadUInt32();               // ("cpu_id", ctypes.c_uint),                   Start at 104, read 4 bytes              POS:  108
            ProgramName = reader.ReadChars(64);        // ("program_name", ctypes.c_char * 64),        Start at 108, read 64 bytes             POS:  172
            HostInfo = reader.ReadChars(1024);         // ("host_info", ctypes.c_char * 1024)          Start at 172, read 1024 bytes           POS: 1196
            reader.ReadBytes(4);                       //                                              Skip 4 bytes due to alignment rules     POS: 1200
        }
    }
}
