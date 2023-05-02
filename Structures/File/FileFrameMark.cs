using System.Text;

namespace ParaTracyReplay.Structures.File {
    /// <summary>
    /// Represents a frame mark event inside of the data file.
    /// </summary>
    internal class FileFrameMark : StructureBase {
        /// <summary>
        /// The name of the marker, expressed as a <see cref="uint"/> pointer.
        /// </summary>
        public uint Name { get; set; }

        /// <summary>
        /// The timestamp of the marker, expressed as a <see cref="long"/>.
        /// </summary>
        public long Timestamp { get; set; }

        /// <inheritdoc/>
        public override byte[] Write() {
            MemoryStream stream = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
                writer.Write(Name);
                // Write out some padding
                writer.Write(new byte[4]);
                writer.Write(Timestamp);
            }

            return stream.ToArray();
        }

        /// <inheritdoc/>
        public override void Read(BinaryReader reader) {
            Name = reader.ReadUInt32();
            // Skip padding bytes
            reader.ReadBytes(4);
            Timestamp = reader.ReadInt64();
        }
    }
}
