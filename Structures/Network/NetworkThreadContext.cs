using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParaTracyReplay.Structures.Network {
    internal class NetworkThreadContext : StructureBase {
        public byte Type { get; set; }
        public uint ThreadId { get; set; }

        /// <inheritdoc/>
        public override byte[] Write() {
            MemoryStream stream = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
                writer.Write(Type);
                writer.Write(ThreadId);
            }

            return stream.ToArray();
        }

        /// <inheritdoc/>
        public override void Read(BinaryReader reader) {
            Type = reader.ReadByte();
            ThreadId = reader.ReadUInt32();
        }
    }
}
