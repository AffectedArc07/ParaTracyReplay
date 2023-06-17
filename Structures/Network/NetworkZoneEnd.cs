using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures.Network
{
    sealed class NetworkZoneEnd : StructureBase
    {
        public override int WriteSize => 9;

        public byte Type { get; set; }
        public long Timestamp { get; set; }

        /// <inheritdoc/>
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
            await writer.WriteAsync(Type);
            await writer.WriteAsync(Timestamp);
        }

        /// <inheritdoc/>
        public override async ValueTask ReadImpl(AsyncBinaryReader reader)
        {
            Type = await reader.ReadByteAsync();
            Timestamp = await reader.ReadInt64Async();
        }
    }
}
