using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures.Network
{
    sealed class NetworkThreadContext : StructureBase
    {
        public override int WriteSize => 5;

        public byte Type { get; set; }
        public uint ThreadId { get; set; }

        /// <inheritdoc/>
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
            await writer.WriteAsync(Type);
            await writer.WriteAsync(ThreadId);
        }

        /// <inheritdoc/>
        public override async ValueTask ReadImpl(AsyncBinaryReader reader)
        {
            Type = await reader.ReadByteAsync();
            ThreadId = await reader.ReadUInt32Async();
        }
    }
}
