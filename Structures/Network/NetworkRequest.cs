using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures.Network
{
    internal class NetworkRequest : StructureBase
    {
        public byte Type { get; set; }
        public long Pointer { get; set; }
        public uint Extra { get; set; }

        /// <inheritdoc/>
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
            await writer.WriteAsync(Type);
            await writer.WriteAsync(Pointer);
            await writer.WriteAsync(Extra);
        }

        /// <inheritdoc/>
        public override async ValueTask ReadImpl(AsyncBinaryReader reader)
        {
            Type = await reader.ReadByteAsync();
            Pointer = await reader.ReadInt64Async();
            Extra = await reader.ReadUInt32Async();
        }
    }
}
