﻿namespace Thrift.Net.Compiler
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
        public static int Main(FileInfo input, DirectoryInfo outputDirectory, IConsole console = null)
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

            using (var stream = input.OpenRead())
            {
                var result = Compiler.Compile(stream);

                OutputCompilationSummary(console, result);

                // TODO: Pull message formatting out to its own object.
                foreach (var message in result.Messages.OrderBy(message => message.LineNumber))
                {
                    console.Out.Write($"{input.Name}({message.LineNumber},{message.StartPosition}-{message.EndPosition}): {message.MessageType} {FormatMessageId(message.MessageId)}: {GetMessage(message.MessageId)} [{input.FullName}]{Environment.NewLine}");
                }

                if (result.HasErrors)
                {
                    return (int)ExitCode.CompilationFailed;
                }
            }

            return (int)ExitCode.Success;
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

        private static string GetMessage(CompilerMessageId messageId)
        {
            return messageId switch
            {
                CompilerMessageId.EnumMustHaveAName
                    => "An enum name must be specified",
                CompilerMessageId.EnumMemberMustHaveAName
                    => "An enum member must have a name",
                CompilerMessageId.EnumValueMustNotBeNegative
                    => "The enum value must not be negative",
                CompilerMessageId.EnumValueMustBeAnInteger
                    => "The enum value must be an integer",
                CompilerMessageId.EnumValueMustBeSpecified
                    => "An enum value must be specified",
                _ => "Message Id {(int)messageId} was not found.",
            };
        }

        private static string FormatMessageId(CompilerMessageId messageId)
        {
            return $"TC{(int)messageId:0000}";
        }
    }
}
