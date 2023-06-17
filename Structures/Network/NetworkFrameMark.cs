using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures.Network
{
    internal class NetworkFrameMark : StructureBase
    {
        public override int WriteSize => 17;

		public byte Type { get; set; }
        public long Timestamp { get; set; }
        public ulong Name { get; set; }

        /// <inheritdoc/>
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
            await writer.WriteAsync(Type);
            await writer.WriteAsync(Timestamp);
            await writer.WriteAsync(Name);
        }

        /// <inheritdoc/>
        public override async ValueTask ReadImpl(AsyncBinaryReader reader)
        {
            Type = await reader.ReadByteAsync();
            Timestamp = await reader.ReadInt64Async();
            Name = await reader.ReadUInt64Async();
        }
    }
}
