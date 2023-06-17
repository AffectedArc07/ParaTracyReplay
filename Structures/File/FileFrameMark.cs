using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures.File
{
    /// <summary>
    /// Represents a frame mark event inside of the data file.
    /// </summary>
    sealed class FileFrameMark : StructureBase
    {
        public override int WriteSize => 16;

        /// <summary>
        /// The name of the marker, expressed as a <see cref="uint"/> pointer.
        /// </summary>
        public uint Name { get; set; }

        /// <summary>
        /// The timestamp of the marker, expressed as a <see cref="long"/>.
        /// </summary>
        public long Timestamp { get; set; }

        /// <inheritdoc/>
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
            await writer.WriteAsync(Name);
            // Write out some padding
            await writer.WriteAsync(0U);
            await writer.WriteAsync(Timestamp);
        }

        /// <inheritdoc/>
        public override async ValueTask ReadImpl(AsyncBinaryReader reader)
        {
            Name = await reader.ReadUInt32Async();
            // Skip padding bytes
            reader.BaseStream.Seek(4, SeekOrigin.Current);
            Timestamp = await reader.ReadInt64Async();
        }
    }
}
