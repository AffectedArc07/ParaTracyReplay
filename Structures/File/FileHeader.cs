using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures
{
    /// <summary>
    /// Represents the header inside of the data file.
    /// </summary>
    sealed class FileHeader : StructureBase
    {
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
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
            await writer.WriteAsync(Signature);
            await writer.WriteAsync(Version);
            await writer.WriteAsync(Multiplier);
            await writer.WriteAsync(InitBegin);
            await writer.WriteAsync(InitEnd);
            await writer.WriteAsync(Delay);
            await writer.WriteAsync(Resolution);
            await writer.WriteAsync(Epoch);
            await writer.WriteAsync(ExecTime);
            await writer.WriteAsync(ProcessId);
            await writer.WriteAsync(SamplingPeriod);
            await writer.WriteAsync(Flags);
            await writer.WriteAsync(CpuArch);
            await writer.WriteAsync(CpuManufacturer);
            await writer.WriteAsync(CpuId);
            await writer.WriteAsync(ProgramName);
            await writer.WriteAsync(HostInfo);
        }

        /// <inheritdoc/>
        public override async ValueTask ReadImpl(AsyncBinaryReader reader)
        {
            //                                                                                                                                            POS:    0
            Signature = await reader.ReadUInt64Async();           // ("signature", ctypes.c_ulonglong),           Start at 0, read 8 bytes                POS:    8
            Version = await reader.ReadUInt32Async();             // ("version", ctypes.c_uint),                  Start at 8, read 4 bytes                POS:   12
            reader.BaseStream.Seek(4, SeekOrigin.Current);        //                                              Skip 4 bytes due to alignment rules     POS:   16
            Multiplier = await reader.ReadDoubleAsync();          // ("multiplier", ctypes.c_double),             Start at 16, read 8 bytes               POS:   24
            InitBegin = await reader.ReadInt64Async();            // ("init_begin", ctypes.c_longlong),           Start at 24, read 8 bytes               POS:   32
            InitEnd = await reader.ReadInt64Async();              // ("init_end", ctypes.c_longlong),             Start at 32, read 8 bytes               POS:   40
            Delay = await reader.ReadInt64Async();                // ("delay", ctypes.c_longlong),                Start at 40, read 8 bytes               POS:   48
            Resolution = await reader.ReadInt64Async();           // ("resolution", ctypes.c_longlong),           Start at 48, read 8 bytes               POS:   56
            Epoch = await reader.ReadInt64Async();                // ("epoch", ctypes.c_longlong),                Start at 56, read 8 bytes               POS:   64
            ExecTime = await reader.ReadInt64Async();             // ("exec_time", ctypes.c_longlong),            Start at 64, read 8 bytes               POS:   72
            ProcessId = await reader.ReadInt64Async();            // ("pid", ctypes.c_longlong),                  Start at 72, read 8 bytes               POS:   80
            SamplingPeriod = await reader.ReadInt64Async();       // ("sampling_period", ctypes.c_longlong),      Start at 80, read 8 bytes               POS:   88
            Flags = await reader.ReadByteAsync();                 // ("flags", ctypes.c_byte),                    Start at 88, read 1 byte                POS:   89
            CpuArch = await reader.ReadByteAsync();               // ("cpu_arch", ctypes.c_byte),                 Start at 89, read 1 byte                POS:   90
            CpuManufacturer = await reader.ReadCharsAsync(12);    // ("cpu_manufacturer", ctypes.c_char * 12),    Start at 90, read 12 bytes              POS:  102
            reader.BaseStream.Seek(2, SeekOrigin.Current);        //                                              Skip 2 bytes due to alignment rules     POS:  104
            CpuId = await reader.ReadUInt32Async();               // ("cpu_id", ctypes.c_uint),                   Start at 104, read 4 bytes              POS:  108
            ProgramName = await reader.ReadCharsAsync(64);        // ("program_name", ctypes.c_char * 64),        Start at 108, read 64 bytes             POS:  172
            HostInfo = await reader.ReadCharsAsync(1024);         // ("host_info", ctypes.c_char * 1024)          Start at 172, read 1024 bytes           POS: 1196
            reader.BaseStream.Seek(4, SeekOrigin.Current);        //                                              Skip 4 bytes due to alignment rules     POS: 1200
        }
    }
}
