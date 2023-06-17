using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures.Network
{
    internal class NetworkSourceLocation : StructureBase
    {
        public override int WriteSize => 1 + 8 + 8 + 8 + 4 + 3;
        public byte Type { get; set; }
        public long Name { get; set; }
        public long Function { get; set; }
        public long File { get; set; }
        public uint Line { get; set; }
        public byte ColourR { get; set; }
        public byte ColourG { get; set; }
        public byte ColourB { get; set; }

        /// <inheritdoc/>
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
            await writer.WriteAsync(Type);
            await writer.WriteAsync(Name);
            await writer.WriteAsync(Function);
            await writer.WriteAsync(File);
            await writer.WriteAsync(Line);
            await writer.WriteAsync(ColourR);
            await writer.WriteAsync(ColourG);
            await writer.WriteAsync(ColourB);
        }

        /// <inheritdoc/>
        public override async ValueTask ReadImpl(AsyncBinaryReader reader)
        {
            Type = await reader.ReadByteAsync();
            Name = await reader.ReadInt64Async();
            Function = await reader.ReadInt64Async();
            File = await reader.ReadInt64Async();
            Line = await reader.ReadUInt32Async();
            ColourR = await reader.ReadByteAsync();
            ColourG = await reader.ReadByteAsync();
            ColourB = await reader.ReadByteAsync();
        }
    }
}
