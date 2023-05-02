using System.Text;

namespace ParaTracyReplay.Structures {
    /// <summary>
    /// Represents a zone begin event inside of the data file.
    /// </summary>
    internal class FileZoneBegin : StructureBase {
        /// <summary>
        /// The ID of the thread we are currently on.
        /// </summary>
        public uint ThreadId { get; set; } 

        /// <summary>
        /// The source location pointer.
        /// </summary>
        public uint SourceLocation { get; set; } 

        /// <summary>
        /// The timestamp this zone begins at.
        /// </summary>
        public long Timestamp { get; set; }

        /// <inheritdoc/>
        public override byte[] Write() {
            MemoryStream stream = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
                writer.Write(ThreadId);
                writer.Write(SourceLocation);
                writer.Write(Timestamp);
            }

            return stream.ToArray();
        }

        /// <inheritdoc/>
        public override void Read(BinaryReader reader) {
            ThreadId = reader.ReadUInt32();
            SourceLocation = reader.ReadUInt32();
            Timestamp = reader.ReadInt64();
        }
    }
}
