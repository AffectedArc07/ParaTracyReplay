using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures.File
{
    /// <summary>
    /// Represents an event inside of the data file.
    /// </summary>
    sealed class FileEvent : StructureBase
    {
        /// <summary>
        /// The contained event in the file.
        /// </summary>
        public StructureBase Event => _backingEvent ?? throw new InvalidOperationException("FileEvent not read!");

        /// <summary>
        /// The type of event in the file, expressed as a <see cref="byte"/>.
        /// </summary>
        public byte Type { get; set; }

        StructureBase? _backingEvent;

        /// <inheritdoc/>
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
            await Event.Write(writer);
            // Write out some padding
            await writer.WriteAsync(new byte[7]);
            await writer.WriteAsync(Type);
        }

        /// <inheritdoc/>
        public override async ValueTask ReadImpl(AsyncBinaryReader reader)
        {
            // Skip the padding
            Type = (await reader.ReadBytesAsync(8))[0];

            // Parse the event type
            // Cast it based on the event type
            _backingEvent = Type switch
            {
                Constants.FileEventZoneBegin => new FileZoneBegin(),
                Constants.FileEventZoneEnd => new FileZoneEnd(),
                Constants.FileEventZoneColour => new FileZoneColour(),
                Constants.FileEventFrameMark => new FileFrameMark(),
                _ => throw new InvalidOperationException($"Unknown event type: {Type}"),
            };

            // And read the data in
            await Event.ReadImpl(reader);
        }
    }
}
