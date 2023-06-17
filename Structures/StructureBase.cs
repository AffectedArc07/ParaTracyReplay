using Overby.Extensions.AsyncBinaryReaderWriter;

namespace ParaTracyReplay.Structures
{
    /// <summary>
    /// Base class for all structures in the program.
    /// This allows for designated read + write methods to interact with byte arrays from C# objects.
    /// </summary>
    abstract class StructureBase 
    {
        /// <summary>
        /// Prepares the data to be written to another stream as a <see cref="byte"/> array.
        /// </summary>
        /// <param name="stream">The <see cref="AsyncBinaryWriter"/> to write to.</param>
        /// <returns>A <see cref="ValueTask"/> representing the running operation.</returns>
        public abstract ValueTask Write(AsyncBinaryWriter writer);

        /// <summary>
        /// Reads the data from the supplied <see cref="BinaryReader"/> into the structure variables.
        /// </summary>
        /// <param name="reader">The <see cref="AsyncBinaryReader"/> to read from.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> resulting in <see langword="true"/> if the read completed successfully, <see langword="false"/> if it reached the end of the read stream.</returns>
        public async ValueTask<bool> Read(AsyncBinaryReader reader)
        {
            try
            {
                await ReadImpl(reader);
                return true;
            }
            catch (EndOfStreamException)
            {
                return false;
            }
        }

        /// <summary>
        /// Reads the data from the supplied <see cref="BinaryReader"/> into the structure variables.
        /// </summary>
        /// <param name="reader">The <see cref="AsyncBinaryReader"/> to read from.</param>
        /// <returns>A <see cref="ValueTask"/> representing the running operation.</returns>
        /// <exception cref="EndOfStreamException">When a read was attempted and the end of the read stream was reached.</exception>
        public abstract ValueTask ReadImpl(AsyncBinaryReader reader);
    }
}
