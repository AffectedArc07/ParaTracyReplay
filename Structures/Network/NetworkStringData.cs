using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures.Network
{
    sealed class NetworkStringData : StructureBase
    {
		public override int WriteSize => 1 + 8 + 2 + String.Length;

		public byte Type { get; set; }
        public ulong Pointer { get; set; }
        public char[] String { get; set; } = Array.Empty<char>();

        /// <inheritdoc/>
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
            await writer.WriteAsync(Type);
            await writer.WriteAsync(Pointer);
            await writer.WriteAsync((ushort)String.Length);
            await writer.WriteAsync(String);
        }

        /// <inheritdoc/>
        public override async ValueTask ReadImpl(AsyncBinaryReader reader)
        {
            Type = await reader.ReadByteAsync();
            Pointer = await reader.ReadUInt64Async();
            var stringLength = await reader.ReadUInt16Async();
            String = await reader.ReadCharsAsync(stringLength);
        }
    }
}
