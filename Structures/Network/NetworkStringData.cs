using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParaTracyReplay.Structures.Network {
    internal class NetworkStringData : StructureBase {
        public byte Type { get; set; }
        public ulong Pointer { get; set; }
        public ushort StringLength { get; set; }
        public char[] String { get; set; }

        /// <inheritdoc/>
        public override byte[] Write() {
            MemoryStream stream = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
                writer.Write(Type);
                writer.Write(Pointer);
                writer.Write(StringLength);
                writer.Write(String);
            }

            return stream.ToArray();
        }

        /// <inheritdoc/>
        public override void Read(BinaryReader reader) {
            Type = reader.ReadByte();
            Pointer = reader.ReadUInt64();
            StringLength = reader.ReadUInt16();
            String = reader.ReadChars(StringLength);
        }
    }
}
