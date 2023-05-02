using System.Text;

namespace ParaTracyReplay.Structures.File {
    /// <summary>
    /// Represents a zone colour event inside of the data file.
    /// </summary>
    internal class FileZoneColour : StructureBase {
        /// <summary>
        /// The ID of the thread we are applying this zone event to.
        /// I feel like this might actually be a different ID but oh well.
        /// </summary>
        public uint ThreadId { get; set; }

        /// <summary>
        /// The colour of the zone, expressed as a <see cref="uint"/>.
        /// </summary>
        public uint Colour { get; set; }

        /// <inheritdoc/>
        public override byte[] Write() {
            MemoryStream stream = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
                writer.Write(ThreadId);
                writer.Write(Colour);
            }

            return stream.ToArray();
        }

        /// <inheritdoc/>
        public override void Read(BinaryReader reader) {
            ThreadId = reader.ReadUInt32();
            Colour = reader.ReadUInt32();
        }
    }
}
