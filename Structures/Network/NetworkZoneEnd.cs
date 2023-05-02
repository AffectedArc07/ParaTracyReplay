using System.Text;

namespace ParaTracyReplay.Structures.Network {
    internal class NetworkZoneEnd : StructureBase {
        public byte Type { get; set; }
        public long Timestamp { get; set; }

        /// <inheritdoc/>
        public override byte[] Write() {
            MemoryStream stream = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
                writer.Write(Type);
                writer.Write(Timestamp);
            }

            return stream.ToArray();
        }

        /// <inheritdoc/>
        public override void Read(BinaryReader reader) {
            Type = reader.ReadByte();
            Timestamp = reader.ReadInt64();
        }
    }
}
