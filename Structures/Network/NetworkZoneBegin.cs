using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures.Network
{
    sealed class NetworkZoneBegin : StructureBase
    {
        public byte Type { get; set; }
        public long Timestamp { get; set; }
        public ulong SourceLocation { get; set; }

        /// <inheritdoc/>
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
            await writer.WriteAsync(Type);
            await writer.WriteAsync(Timestamp);
            await writer.WriteAsync(SourceLocation);
        }

        /// <inheritdoc/>
        public override async ValueTask ReadImpl(AsyncBinaryReader reader)
        {
            Type = await reader.ReadByteAsync();
            Timestamp = await reader.ReadInt64Async();
            SourceLocation = await reader.ReadUInt64Async();
        }
    }
}
