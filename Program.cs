using Serilog;

namespace ParaTracyReplay {
    /// <summary>
    /// Main program class
    /// </summary>
    internal class Program {
        /// <summary>
        /// Program entrypoint.
        /// This sets up serilog, validates the args then invokes the <see cref="Loader"/>.
        /// </summary>
        /// <param name="args">The file to load in position 0</param>
        static void Main(string[] args) {
            // Setup serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Logger(lc => {
                    lc.WriteTo.Console(
                        outputTemplate:
                        "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] {Message:lj}{NewLine}{Exception}");
                })
                .CreateLogger();

            // Validate args
            if (args.Length == 0) {
                Log.Logger.Fatal("Error, not enough arguments");
                Log.Logger.Fatal("Usage: ParaTracyReplay.exe yourfile.utracy");
                Environment.Exit(1);
            }

            // Create and invoke the loader
            Loader loader = new Loader();
            loader.Load(args[0]);
        }
    }
}