using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParaTracyReplay.Structures.Network {
    internal class NetworkZoneBegin : StructureBase {
        public byte Type { get; set; }
        public long Timestamp { get; set; }
        public ulong SourceLocation { get; set; }

        /// <inheritdoc/>
        public override byte[] Write() {
            MemoryStream stream = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
                writer.Write(Type);
                writer.Write(Timestamp);
                writer.Write(SourceLocation);
            }

            return stream.ToArray();
        }

        /// <inheritdoc/>
        public override void Read(BinaryReader reader) {
            Type = reader.ReadByte();
            Timestamp = reader.ReadInt64();
            SourceLocation = reader.ReadUInt64();
        }
    }
}
