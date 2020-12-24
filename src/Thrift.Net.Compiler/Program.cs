namespace Thrift.Net.Compiler
{
    using System;
    using System.CommandLine;
    using System.IO;
    using System.Linq;
    using Thrift.Net.Compilation;

    /// <summary>
    /// This class contains the main entry point for the thrift compiler.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Gets or sets the Thrift compiler.
        /// </summary>
        /// <remarks>
        /// Used to allow us to inject a Thrift compiler in during unit testing.
        /// </remarks>
        public static IThriftCompiler Compiler { get; set; } = new ThriftCompiler();

        /// <summary>
        /// Gets or sets the document generator.
        /// </summary>
        /// <remarks>
        /// Used to allow us to inject a document generator in during unit testing.
        /// </remarks>
        public static IThriftDocumentGenerator DocumentGenerator { get; set; } = new ThriftDocumentGenerator();

        /// <summary>
        /// Gets or sets the file provider.
        /// </summary>
        /// <remarks>
        /// Used to allow us to inject a file provider in during unit testing.
        /// </remarks>
        public static IThriftFileProvider FileProvider { get; set; } = new ThriftFileProvider();

        /// <summary>
        /// Gets or sets the file writer.
        /// </summary>
        /// <remarks>
        /// Used to allow us to inject a file provider in during unit testing.
        /// </remarks>
        public static IThriftFileWriter FileWriter { get; set; } = new ThriftFileWriter();

        /// <summary>
        /// The Thrift.Net Thrift compiler. Use this to generate C# code from
        /// your Thrift IDL files.
        /// </summary>
        /// <param name="input">Specifies the path to the input file.</param>
        /// <param name="outputDirectory">
        /// Specifies the output directory. If it does not already exist it will be created.
        /// </param>
        /// <param name="console">Used to interact with the console.</param>
        /// <returns>
        /// The exit code. This can be used by scripts to determine whether the
        /// compile was successful or not.
        /// </returns>
        public static int Main(FileInfo input, DirectoryInfo outputDirectory, IConsole console)
        {
            try
            {
                if (!input.Exists)
                {
                    console.Error.Write($"The specified input file '{input.Name}' could not be found.{Environment.NewLine}");
                    return (int)ExitCode.InputFileNotFound;
                }

                if (!outputDirectory.Exists)
                {
                    outputDirectory.Create();
                }

                console.Out.Write($"Starting compilation of {input.Name}{Environment.NewLine}");

                var thriftFile = FileProvider.Create(
                    new DirectoryInfo(Directory.GetCurrentDirectory()),
                    input,
                    outputDirectory);

                using (var stream = input.OpenRead())
                {
                    var result = Compiler.Compile(stream);

                    OutputCompilationSummary(console, result);

                    // TODO: Pull message formatting out to its own object.
                    foreach (var message in result.Messages.OrderBy(message => message.LineNumber))
                    {
                        console.Out.Write($"{thriftFile.RelativePath}({message.LineNumber},{message.StartPosition}-{message.EndPosition}): {message.MessageType} {message.FormattedMessageId}: {message.Message} [{input.FullName}]{Environment.NewLine}");
                    }

                    if (result.HasErrors)
                    {
                        return (int)ExitCode.CompilationFailed;
                    }

                    if (result.Document.ContainsDefinitions)
                    {
                        var generatedCode = DocumentGenerator.Generate(result.Document);

                        FileWriter.Write(thriftFile, generatedCode);
                    }
                }

                return (int)ExitCode.Success;
            }
            catch (Exception exception)
            {
                console.Out.Write($"Error: {exception}{Environment.NewLine}{Environment.NewLine}");
                console.Out.Write($"Thrift.Net has encountered an error. This is a problem with the compiler and not caused by your code.{Environment.NewLine}");
                console.Out.Write($"Please help us to resolve this by reporting a new issue here: https://github.com/adamconnelly/Thrift.Net/issues/new?template=issue_template.md. {Environment.NewLine} {Environment.NewLine}");
                console.Out.Write($"We welcome contributions, so please feel free to create a PR to resolve the issue.{Environment.NewLine}");
                console.Out.Write($"You can find out how to contribute here: https://github.com/adamconnelly/Thrift.Net/blob/main/docs/CONTRIBUTING.md. {Environment.NewLine}");
                return (int)ExitCode.UnhandledException;
            }
        }

        private static void OutputCompilationSummary(IConsole console, CompilationResult result)
        {
            if (result.HasErrors)
            {
                if (result.HasWarnings)
                {
                    console.Out.Write($"Compilation failed with {result.Errors.Count()} error(s) and {result.Warnings.Count()} warning(s):{Environment.NewLine}{Environment.NewLine}");
                }
                else
                {
                    console.Out.Write($"Compilation failed with {result.Messages.Count()} error(s):{Environment.NewLine}{Environment.NewLine}");
                }
            }
            else if (result.HasWarnings)
            {
                console.Out.Write($"Compilation succeeded with {result.Warnings.Count()} warning(s):{Environment.NewLine}{Environment.NewLine}");
            }
            else
            {
                console.Out.Write($"Compilation succeeded with no errors or warnings!{Environment.NewLine}");
            }
        }
    }
}
