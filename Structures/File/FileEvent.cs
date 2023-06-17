using System.Diagnostics;

using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures.File
{
    /// <summary>
    /// Represents an event inside of the data file.
    /// </summary>
    sealed class FileEvent : StructureBase
    {
        public override int WriteSize => 8 + Event.WriteSize;

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
            var expectedNewPosition = reader.BaseStream.Position + 24;
            if (expectedNewPosition > reader.BaseStream.Length)
                throw new EndOfStreamException();

            Type = await reader.ReadByteAsync();

            reader.BaseStream.Seek(7, SeekOrigin.Current);

            // Parse the event type
            // Cast it based on the event type
            _backingEvent = Type switch
            {
                Constants.FileEventZoneBegin => new FileZoneBegin(),
                Constants.FileEventZoneEnd => new FileZoneEnd(),
                Constants.FileEventZoneColour => new FileZoneColour(),
                Constants.FileEventFrameMark => new FileFrameMark(),
            };

            // And read the data in
            await Event.ReadImpl(reader);
        }
    }
}
