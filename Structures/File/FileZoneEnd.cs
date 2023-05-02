using System.Text;

namespace ParaTracyReplay.Structures {
    /// <summary>
    /// Represents a zone end event inside of the data file.
    /// </summary>
    internal class FileZoneEnd : StructureBase {
        /// <summary>
        /// The ID of the thread we are applying this zone event to.
        /// I feel like this might actually be a different ID but oh well.
        /// </summary>
        public uint ThreadId { get; set; }

        /// <summary>
        /// The timestamp this zone ends.
        /// </summary>
        public long Timestamp { get; set; }

        /// <inheritdoc/>
        public override byte[] Write() {
            MemoryStream stream = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
                writer.Write(ThreadId);
                // Write 4 padding bytes
                writer.Write(new byte[4]);
                writer.Write(Timestamp);
            }

            return stream.ToArray();
        }

        /// <inheritdoc/>
        public override void Read(BinaryReader reader) {
            ThreadId = reader.ReadUInt32();
            // Skip over the 4 padding bytes
            reader.ReadBytes(4);
            Timestamp = reader.ReadInt64();
        }
    }
}
