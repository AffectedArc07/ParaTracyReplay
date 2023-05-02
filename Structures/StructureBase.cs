namespace ParaTracyReplay.Structures {
    /// <summary>
    /// Base class for all structures in the program.
    /// This allows for designated read + write methods to interact with byte arrays from C# objects.
    /// </summary>
    internal abstract class StructureBase {
        /// <summary>
        /// Prepares the data to be written to another stream as a <see cref="byte"/> array.
        /// </summary>
        /// <returns>A <see cref="byte"/> array containing all the variables from this structure.</returns>
        public abstract byte[] Write();

        /// <summary>
        /// Reads the data from the supplied <see cref="BinaryReader"/> into the structure variables.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
        public abstract void Read(BinaryReader reader);
    }
}
