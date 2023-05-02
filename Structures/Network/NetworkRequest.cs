using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParaTracyReplay.Structures.Network {
    internal class NetworkRequest : StructureBase {
        public byte Type { get; set; }
        public long Pointer { get; set; }
        public uint Extra { get; set; }

        /// <inheritdoc/>
        public override byte[] Write() {
            MemoryStream stream = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
                writer.Write(Type);
                writer.Write(Pointer);
                writer.Write(Extra);
            }

            return stream.ToArray();
        }

        /// <inheritdoc/>
        public override void Read(BinaryReader reader) {
            Type = reader.ReadByte();
            Pointer = reader.ReadInt64();
            Extra = reader.ReadUInt32();
        }
    }
}
