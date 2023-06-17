using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures.Network
{
    sealed class NetworkZoneColour : StructureBase
    {
        public override int WriteSize => 4;

		public byte Type { get; set; }
        public byte ColourR { get; set; }
        public byte ColourG { get; set; }
        public byte ColourB { get; set; }

        /// <inheritdoc/>
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
            await writer.WriteAsync(Type);
            await writer.WriteAsync(ColourR);
            await writer.WriteAsync(ColourG);
            await writer.WriteAsync(ColourB);
        }

        /// <inheritdoc/>
        public override async ValueTask ReadImpl(AsyncBinaryReader reader)
        {
            Type = await reader.ReadByteAsync();
            ColourR = await reader.ReadByteAsync();
            ColourG = await reader.ReadByteAsync();
            ColourB = await reader.ReadByteAsync();
        }
    }
}
