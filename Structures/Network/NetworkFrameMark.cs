using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParaTracyReplay.Structures.Network {
    internal class NetworkFrameMark : StructureBase {
        public byte Type { get; set; }
        public long Timestamp { get; set; }
        public ulong Name { get; set; }

        /// <inheritdoc/>
        public override byte[] Write() {
            MemoryStream stream = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
                writer.Write(Type);
                writer.Write(Timestamp);
                writer.Write(Name);
            }

            return stream.ToArray();
        }

        /// <inheritdoc/>
        public override void Read(BinaryReader reader) {
            Type = reader.ReadByte();
            Timestamp = reader.ReadInt64();
            Name = reader.ReadUInt64();
        }
    }
}
