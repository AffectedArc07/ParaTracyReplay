using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures.Network
{
    sealed class NetworkStringData : StructureBase
    {
        public byte Type { get; set; }
        public ulong Pointer { get; set; }
        public ushort StringLength { get; set; }
        public char[] String { get; set; } = Array.Empty<char>();

        /// <inheritdoc/>
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
            await writer.WriteAsync(Type);
            await writer.WriteAsync(Pointer);
            await writer.WriteAsync(StringLength);
            await writer.WriteAsync(String);
        }

        /// <inheritdoc/>
        public override async ValueTask ReadImpl(AsyncBinaryReader reader)
        {
            Type = await reader.ReadByteAsync();
            Pointer = await reader.ReadUInt64Async();
            StringLength = await reader.ReadUInt16Async();
            String = await reader.ReadCharsAsync(StringLength);
        }
    }
}
