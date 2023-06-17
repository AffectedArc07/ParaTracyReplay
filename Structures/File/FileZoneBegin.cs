using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures
{
    /// <summary>
    /// Represents a zone begin event inside of the data file.
    /// </summary>
    sealed class FileZoneBegin : StructureBase
    {
        public override int WriteSize => 16;

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
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
            await writer.WriteAsync(ThreadId);
            await writer.WriteAsync(SourceLocation);
            await writer.WriteAsync(Timestamp);
        }

        /// <inheritdoc/>
        public override async ValueTask ReadImpl(AsyncBinaryReader reader)
        {
            ThreadId = await reader.ReadUInt32Async();
            SourceLocation = await reader.ReadUInt32Async();
            Timestamp = await reader.ReadInt64Async();
        }
    }
}
