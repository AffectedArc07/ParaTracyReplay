using System.Text;

namespace ParaTracyReplay.Structures.Network {
    internal class NetworkZoneColour : StructureBase {
        public byte Type { get; set; }
        public byte ColourR { get; set; }
        public byte ColourG { get; set; }
        public byte ColourB { get; set; }

        /// <inheritdoc/>
        public override byte[] Write() {
            MemoryStream stream = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
                writer.Write(Type);
                writer.Write(ColourR);
                writer.Write(ColourG);
                writer.Write(ColourB);
            }

            return stream.ToArray();
        }

        /// <inheritdoc/>
        public override void Read(BinaryReader reader) {
            Type = reader.ReadByte();
            ColourR = reader.ReadByte();
            ColourG = reader.ReadByte();
            ColourB = reader.ReadByte();
        }
    }
}
