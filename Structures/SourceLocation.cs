namespace ParaTracyReplay.Structures {
    /// <summary>
    /// Represents a source location to be sent to Tracy.
    /// </summary>
    internal class SourceLocation {
        /// <summary>
        /// The zone name, expressed as a <see cref="long"/> pointer.
        /// </summary>
        public long Name { get; set; }

        /// <summary>
        /// The function name, expressed as a <see cref="long"/> pointer.
        /// </summary>
        public long Function { get; set; }

        /// <summary>
        /// The file name, expressed as a <see cref="long"/> pointer.
        /// </summary>
        public long File { get; set; }

        /// <summary>
        /// The line of the file, expressed as a <see cref="uint"/>.
        /// </summary>
        public uint Line { get; set; }

        /// <summary>
        /// The colour of the zone, expressed as a <see cref="uint"/>.
        /// </summary>
        public uint Colour { get; set; }
    }
}
