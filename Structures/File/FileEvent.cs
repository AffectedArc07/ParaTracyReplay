using System.Text;

namespace ParaTracyReplay.Structures.File {
    /// <summary>
    /// Represents an event inside of the data file.
    /// </summary>
    internal class FileEvent : StructureBase {
        /// <summary>
        /// The contained event in the file.
        /// </summary>
        public StructureBase Event { get; set; }

        /// <summary>
        /// The type of event in the file, expressed as a <see cref="byte"/>.
        /// </summary>
        public byte Type { get; set; }

        /// <inheritdoc/>
        public override byte[] Write() {
            MemoryStream stream = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
                writer.Write(Event.Write());
                // Write out some padding
                writer.Write(new byte[7]);
                writer.Write(Type);
            }

            return stream.ToArray();
        }

        /// <inheritdoc/>
        public override void Read(BinaryReader reader) {
            // Get our type
            Type = reader.ReadByte();

            // Skip the padding
            reader.ReadBytes(7);

            // Parse the event type
            switch (Type) {
                // Cast it based on the event type
                case Constants.FileEventZoneBegin:
                    Event = new FileZoneBegin();
                    break;
                case Constants.FileEventZoneEnd:
                    Event = new FileZoneEnd();
                    break;
                case Constants.FileEventZoneColour:
                    Event = new FileZoneColour();
                    break;
                case Constants.FileEventFrameMark:
                    Event = new FileFrameMark();
                    break;
            }

            // And read the data in
            Event.Read(reader);
        }
    }
}
