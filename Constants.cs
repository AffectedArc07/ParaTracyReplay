namespace ParaTracyReplay {
    /// <summary>
    /// Contains a bunch of constants Tracy needs.
    /// </summary>
    internal class Constants {
        // File protocol stuff

        /// <summary>
        /// The expected file signature that this program is designed to decode.
        /// </summary>
        public const ulong FileSignature = 0x6D64796361727475;

        /// <summary>
        /// The expected file version that this program is designed to decode.
        /// </summary>
        public const uint FileVersion = 2;

        /// <summary>
        /// The event ID for a zone begin in the file.
        /// </summary>
        public const byte FileEventZoneBegin = 15;

        /// <summary>
        /// The event ID for a zone end in the file.
        /// </summary>
        public const byte FileEventZoneEnd = 17;

        /// <summary>
        /// The event ID for a zone colour in the file.
        /// </summary>
        public const byte FileEventZoneColour = 62;

        /// <summary>
        /// The event ID for a marked frame in the file.
        /// </summary>
        public const byte FileEventFrameMark = 64;

        // Network protocol stuff
        /// <summary>
        /// The maximum size of a network frame that the Tracy client can capture.
        /// </summary>
        public const int NetworkMaxFrameSize = 256 * 1024;

        /// <summary>
        /// Packet ID for a welcome handshake.
        /// </summary>
        public const byte NetworkHandshakeWelcome = 0x01;

        /// <summary>
        /// Packet ID for a protocol mismatch.
        /// </summary>
        public const byte NetworkHandshakeProtocolMismatch = 0x02;

        /// <summary>
        /// Packet ID for a zone begin packet.
        /// </summary>
        public const byte NetworkEventZoneBegin = 15;

        /// <summary>
        /// Packet ID for a zone end packet.
        /// </summary>
        public const byte NetworkEventZoneEnd = 17;

        /// <summary>
        /// Packet ID for a termination packet.
        /// </summary>
        public const byte NetworkEventTerminate = 55;
        
        /// <summary>
        /// Packet ID for a thread context packet.
        /// </summary>
        public const byte NetworkEventThreadContext = 57;

        /// <summary>
        /// Packet ID for a zone colour packet.
        /// </summary>
        public const byte NetworkEventZoneColour = 62;

        /// <summary>
        /// Packet ID for a frame mark packet.
        /// </summary>
        public const byte NetworkEventFrameMark = 64;

        /// <summary>
        /// Packet ID for a source location packet.
        /// </summary>
        public const byte NetworkEventSrcloc = 67;

        /// <summary>
        /// Packet ID for a response to a no-op query.
        /// </summary>
        public const byte NetworkResponseServerQueryNoop = 87;

        /// <summary>
        /// Packet ID for a response to a source code request to say it isnt available.
        /// </summary>
        public const byte NetworkResponseSourceCodeNotAvailable = 88;

        /// <summary>
        /// Packet ID for a response to a source code request to say it isnt available.
        /// </summary>
        public const byte NetworkResponseSymbolCodeNotAvailable = 89;

        /// <summary>
        /// Packet ID for a response containing string data.
        /// </summary>
        public const byte NetworkResponseStringData = 94;

        /// <summary>
        /// Packet ID for a response containing a thread name.
        /// </summary>
        public const byte NetworkResponseThreadName = 95;

        /// <summary>
        /// Packet ID for a request about a termination.
        /// </summary>
        public const byte NetworkQueryTerminate = 0;

        /// <summary>
        /// Packet ID for a request about a string.
        /// </summary>
        public const byte NetworkQueryString = 1;

        /// <summary>
        /// Packet ID for a request about a thread string.
        /// </summary>
        public const byte NetworkQueryThreadString = 2;

        /// <summary>
        /// Packet ID for a request about a source location.
        /// </summary>
        public const byte NetworkQuerySrcloc = 3;

        /// <summary>
        /// Packet ID for a request about a plot name.
        /// </summary>
        public const byte NetworkQueryPlotName = 4;

        /// <summary>
        /// Packet ID for a request about a frame name.
        /// </summary>
        public const byte NetworkQueryFrameName = 5;

        /// <summary>
        /// Packet ID for a request about a query parameter.
        /// </summary>
        public const byte NetworkQueryParameter = 6;

        /// <summary>
        /// Packet ID for a request about a fiber name.
        /// </summary>
        public const byte NetworkQueryFiberName = 7;

        /// <summary>
        /// Packet ID for a disconnect.
        /// </summary>
        public const byte NetworkQueryDisconnect = 8;

        /// <summary>
        /// Packet ID for a request about a callstack frame.
        /// </summary>
        public const byte NetworkQueryCallstackFrame = 9;

        /// <summary>
        /// Packet ID for a request about an external name.
        /// </summary>
        public const byte NetworkQueryExternalName = 10;

        /// <summary>
        /// Packet ID for a request about a symbol.
        /// </summary>
        public const byte NetworkQuerySymbol = 11;

        /// <summary>
        /// Packet ID for a request about symbol code.
        /// </summary>
        public const byte NetworkQuerySymbolCode = 12;

        /// <summary>
        /// Packet ID for a request about a code location.
        /// </summary>
        public const byte NetworkQueryCodeLocation = 13;

        /// <summary>
        /// Packet ID for a request about source code.
        /// </summary>
        public const byte NetworkQuerySourceCode = 14;

        /// <summary>
        /// Packet ID for a request about data transfer.
        /// </summary>
        public const byte NetworkQueryDataTransfer = 15;

        /// <summary>
        /// Packet ID for a request about partial data transfer.
        /// </summary>
        public const byte NetworkQueryDataTransferPart = 16;
    }
}
