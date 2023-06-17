using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures
{
    /// <summary>
    /// Represents a zone end event inside of the data file.
    /// </summary>
    sealed class FileZoneEnd : StructureBase
    {
        public override int WriteSize => 16;

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
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
            await writer.WriteAsync(ThreadId);
            // Write 4 padding bytes
            await writer.WriteAsync(0U);
            await writer.WriteAsync(Timestamp);
        }

        /// <inheritdoc/>
        public override async ValueTask ReadImpl(AsyncBinaryReader reader)
        {
            ThreadId = await reader.ReadUInt32Async();
            // Skip over the 4 padding bytes
            await reader.ReadBytesAsync(4);
            Timestamp = await reader.ReadInt64Async();
        }
    }
}
