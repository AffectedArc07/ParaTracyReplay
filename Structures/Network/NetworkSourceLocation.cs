using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParaTracyReplay.Structures.Network {
    internal class NetworkSourceLocation : StructureBase {
        public byte Type { get; set; }
        public long Name { get; set; }
        public long Function { get; set; }
        public long File { get; set; }
        public uint Line { get; set; }
        public byte ColourR { get; set; }
        public byte ColourG { get; set; }
        public byte ColourB { get; set; }

        /// <inheritdoc/>
        public override byte[] Write() {
            MemoryStream stream = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
                writer.Write(Type);
                writer.Write(Name);
                writer.Write(Function);
                writer.Write(File);
                writer.Write(Line);
                writer.Write(ColourR);
                writer.Write(ColourG);
                writer.Write(ColourB);
            }

            return stream.ToArray();
        }

        /// <inheritdoc/>
        public override void Read(BinaryReader reader) {
            Type = reader.ReadByte();
            Name = reader.ReadInt64();
            Function = reader.ReadInt64();
            File = reader.ReadInt64();
            Line = reader.ReadUInt32();
            ColourR = reader.ReadByte();
            ColourG = reader.ReadByte();
            ColourB = reader.ReadByte();
        }
    }
}
