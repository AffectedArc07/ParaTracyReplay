using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParaTracyReplay.Structures.Network {
    internal class NetworkHeader : StructureBase {
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

        public NetworkHeader() {
            CpuManufacturer = new char[12];
            ProgramName = new char[64];
            HostInfo = new char[1024];
        }

        /// <inheritdoc/>
        public override byte[] Write() {
            MemoryStream stream = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
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
            Multiplier = reader.ReadDouble();
            InitBegin = reader.ReadInt64();
            InitEnd = reader.ReadInt64();
            Delay = reader.ReadInt64();
            Resolution = reader.ReadInt64();
            Epoch = reader.ReadInt64();
            ExecTime = reader.ReadInt64();
            ProcessId = reader.ReadInt64();
            SamplingPeriod = reader.ReadInt64();
            Flags = reader.ReadSByte();
            CpuArch = reader.ReadSByte();
            CpuManufacturer = reader.ReadChars(12);
            CpuId = reader.ReadUInt32();
            ProgramName = reader.ReadChars(64);
            HostInfo = reader.ReadChars(1024);
        }
    }
}
