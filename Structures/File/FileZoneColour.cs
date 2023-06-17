using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures.File
{
    /// <summary>
    /// Represents a zone colour event inside of the data file.
    /// </summary>
    sealed class FileZoneColour : StructureBase
    {
        public override int WriteSize => 8;

		/// <summary>
		/// The ID of the thread we are applying this zone event to.
		/// I feel like this might actually be a different ID but oh well.
		/// </summary>
		public uint ThreadId { get; set; }

        /// <summary>
        /// The colour of the zone, expressed as a <see cref="uint"/>.
        /// </summary>
        public uint Colour { get; set; }

        /// <inheritdoc/>
        public override async ValueTask Write(AsyncBinaryWriter writer)
        {
            await writer.WriteAsync(ThreadId);
            await writer.WriteAsync(Colour);
        }

        /// <inheritdoc/>
        public override async ValueTask ReadImpl(AsyncBinaryReader reader)
        {
            ThreadId = await reader.ReadUInt32Async();
            Colour = await reader.ReadUInt32Async();
        }
    }
}
