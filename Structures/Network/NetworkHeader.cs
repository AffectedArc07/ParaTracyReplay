using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures.Network
{
    internal class NetworkHeader : StructureBase
    {
        public override int WriteSize => sizeof(double)
            + sizeof(long)
            + sizeof(long)
            + sizeof(long)
            + sizeof(long)
            + sizeof(long)
            + sizeof(long)
            + sizeof(long)
            + sizeof(long)
            + 1
            + 12
            + 4
            + 64
            + 1024;

        public double Multiplier { get; set; }
        public long InitBegin { get; set; }
        public long InitEnd { get; set; }
        public long Delay { get; set; }
        public long Resolution { get; set; }
        public long Epoch { get; set; }
        public long ExecTime { get; set; }
        public long ProcessId { get; set; }
        public long SamplingPeriod { get; set; }
        public sbyte Flags { get; set; }
        public sbyte CpuArch { get; set; }
        public char[] CpuManufacturer { get; set; }
        public uint CpuId { get; set; }
        public char[] ProgramName { get; set; }
        public char[] HostInfo { get; set; }

        public NetworkHeader()
        {
            CpuManufacturer = new char[12];
            ProgramName = new char[64];
            HostInfo = new char[1024];
        }

        /// <inheritdoc/>
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
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
            Multiplier = await reader.ReadDoubleAsync();
            InitBegin = await reader.ReadInt64Async();
            InitEnd = await reader.ReadInt64Async();
            Delay = await reader.ReadInt64Async();
            Resolution = await reader.ReadInt64Async();
            Epoch = await reader.ReadInt64Async();
            ExecTime = await reader.ReadInt64Async();
            ProcessId = await reader.ReadInt64Async();
            SamplingPeriod = await reader.ReadInt64Async();
            Flags = await reader.ReadSByteAsync();
            CpuArch = await reader.ReadSByteAsync();
            CpuManufacturer = await reader.ReadCharsAsync(12);
            CpuId = await reader.ReadUInt32Async();
            ProgramName = await reader.ReadCharsAsync(64);
            HostInfo = await reader.ReadCharsAsync(1024);
        }
    }
}
