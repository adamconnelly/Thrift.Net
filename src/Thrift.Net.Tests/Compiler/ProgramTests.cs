namespace Thrift.Net.Tests.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.IO;
    using System.Linq;
    using NSubstitute;
    using Thrift.Net.Compilation;
    using Thrift.Net.Compilation.Model;
    using Thrift.Net.Compiler;
    using Xunit;

    public class ProgramTests : IDisposable
    {
        private readonly List<FileInfo> tempFiles = new List<FileInfo>();
        private readonly List<DirectoryInfo> tempDirectories = new List<DirectoryInfo>();
        private readonly IConsole console = Substitute.For<IConsole>();
        private readonly IThriftCompiler compiler = Substitute.For<IThriftCompiler>();

        public ProgramTests()
        {
            Program.Compiler = this.compiler;
        }

        public void Dispose()
        {
            // Refresh temp files and directories to make sure the Exists check
            // are up to date.
            this.tempFiles.ForEach(file => file.Refresh());
            this.tempDirectories.ForEach(directory => directory.Refresh());

            // Delete any temp files and directories that still exist. It's not
            // the end of the world if this fails since we're creating the files
            // in the temp folder, but we want to try to avoid filling it up
            // unnecessarily.
            this.tempFiles
                .Where(file => file.Exists)
                .ToList()
                .ForEach(file => file.Delete());

            this.tempDirectories
                .Where(directory => directory.Exists)
                .ToList()
                .ForEach(directory => directory.Delete(true));
        }

        [Fact]
        public void When_InputFileDoesNotExist_ReturnsError()
        {
            // Arrange
            var inputFile = new FileInfo("missing-file.thrift");
            var outputDirectory = this.GetTempDirectory();

            // Act
            var result = Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            Assert.Equal((int)ExitCode.InputFileNotFound, result);
        }

        [Fact]
        public void When_InputFileDoesNotExist_OutputsErrorMessage()
        {
            // Arrange
            var inputFile = new FileInfo("missing-file.thrift");
            var outputDirectory = this.GetTempDirectory();

            // Act
            Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            this.console.Error.Received().Write(
                "The specified input file 'missing-file.thrift' could not be found." + Environment.NewLine);
        }

        [Fact]
        public void When_InputFileExists_ReturnsSuccess()
        {
            // Arrange
            var inputFile = this.CreateTempFile();
            var outputDirectory = this.GetTempDirectory();
            this.SetupSuccessfulCompilation();

            // Act
            var result = Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            Assert.Equal((int)ExitCode.Success, result);
        }

        [Fact]
        public void When_OutputDirectoryDoesNotExist_CreatesDirectory()
        {
            // Arrange
            var inputFile = this.CreateTempFile();
            var outputDirectory = this.GetTempDirectory();
            this.SetupSuccessfulCompilation();

            // Act
            Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            outputDirectory.Refresh();
            Assert.True(outputDirectory.Exists);
        }

        [Fact]
        public void When_CompilationHasErrors_ReturnsFailure()
        {
            // Arrange
            var inputFile = this.CreateTempFile();
            var outputDirectory = this.GetTempDirectory();
            this.SetupCompilationWithError();

            // Act
            var result = Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            Assert.Equal((int)ExitCode.CompilationFailed, result);
        }

        [Fact]
        public void When_CompilationHasWarnings_ReturnsSuccess()
        {
            // Arrange
            var inputFile = this.CreateTempFile();
            var outputDirectory = this.GetTempDirectory();
            this.SetupCompilationWithWarning();

            // Act
            var result = Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            Assert.Equal((int)ExitCode.Success, result);
        }

        [Fact]
        public void When_CompilationHasMessages_OutputsMessages()
        {
            // Arrange
            var inputFile = this.CreateTempFile();
            var outputDirectory = this.GetTempDirectory();
            this.SetupCompilationWithMessages(
                new CompilationMessage(CompilerMessageId.EnumMustHaveAName, CompilerMessageType.Error, 1, 1, 4),
                new CompilationMessage(CompilerMessageId.EnumMemberMustHaveAName, CompilerMessageType.Error, 2, 4, 8));

            // Act
            Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            var enumMustHaveName = $"TC{(int)CompilerMessageId.EnumMustHaveAName:0000}";
            var enumMemberMustHaveName = $"TC{(int)CompilerMessageId.EnumMemberMustHaveAName:0000}";
            this.console.Out.Received().Write(
                $"{inputFile.Name}(1,1-4): Error {enumMustHaveName}: An enum name must be specified [{inputFile.FullName}]{Environment.NewLine}");
            this.console.Out.Received().Write(
                $"{inputFile.Name}(2,4-8): Error {enumMemberMustHaveName}: An enum member must have a name [{inputFile.FullName}]{Environment.NewLine}");
        }

        [Fact]
        public void When_CompilationHasErrors_OutputsErrorCount()
        {
            // Arrange
            var inputFile = this.CreateTempFile();
            var outputDirectory = this.GetTempDirectory();
            this.SetupCompilationWithMessages(
                new CompilationMessage(CompilerMessageId.EnumMustHaveAName, CompilerMessageType.Error, 1, 1, 4),
                new CompilationMessage(CompilerMessageId.EnumMemberMustHaveAName, CompilerMessageType.Error, 2, 4, 8));

            // Act
            Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            this.console.Out.Received().Write($"Compilation failed with 2 error(s):{Environment.NewLine}{Environment.NewLine}");
        }

        [Fact]
        public void When_CompilationHasErrorsAndWarnings_OutputsErrorCount()
        {
            // Arrange
            var inputFile = this.CreateTempFile();
            var outputDirectory = this.GetTempDirectory();
            this.SetupCompilationWithMessages(
                new CompilationMessage(CompilerMessageId.EnumMustHaveAName, CompilerMessageType.Error, 1, 1, 4),
                new CompilationMessage(CompilerMessageId.EnumMemberMustHaveAName, CompilerMessageType.Error, 2, 4, 8),
                new CompilationMessage(CompilerMessageId.EnumMemberMustHaveAName, CompilerMessageType.Warning, 2, 4, 8));

            // Act
            Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            this.console.Out.Received().Write($"Compilation failed with 2 error(s) and 1 warning(s):{Environment.NewLine}{Environment.NewLine}");
        }

        [Fact]
        public void When_CompilationHasWarnings_OutputsWarningCount()
        {
            // Arrange
            var inputFile = this.CreateTempFile();
            var outputDirectory = this.GetTempDirectory();
            this.SetupCompilationWithMessages(
                new CompilationMessage(CompilerMessageId.EnumMustHaveAName, CompilerMessageType.Warning, 1, 1, 4),
                new CompilationMessage(CompilerMessageId.EnumMemberMustHaveAName, CompilerMessageType.Warning, 2, 4, 8),
                new CompilationMessage(CompilerMessageId.EnumMemberMustHaveAName, CompilerMessageType.Warning, 2, 4, 8));

            // Act
            Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            this.console.Out.Received().Write($"Compilation succeeded with 3 warning(s):{Environment.NewLine}{Environment.NewLine}");
        }

        [Fact]
        public void When_CompilationHasNoErrorsOrWarnings_OutputsSuccessMessage()
        {
            // Arrange
            var inputFile = this.CreateTempFile();
            var outputDirectory = this.GetTempDirectory();
            this.SetupSuccessfulCompilation();

            // Act
            Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            this.console.Out.Received().Write($"Compilation succeeded with no errors or warnings!{Environment.NewLine}");
        }

        private void SetupSuccessfulCompilation()
        {
            var compilationResult = new CompilationResult(
                new ThriftDocument(new List<EnumDefinition>()),
                new List<CompilationMessage>());

            this.compiler.Compile(default).ReturnsForAnyArgs(compilationResult);
        }

        private void SetupCompilationWithError()
        {
            var errorMessage = new CompilationMessage(
                CompilerMessageId.EnumMustHaveAName, CompilerMessageType.Error, 1, 1, 1);
            var compilationResult = new CompilationResult(
                new ThriftDocument(new List<EnumDefinition>()),
                new List<CompilationMessage> { errorMessage });

            this.compiler.Compile(default).ReturnsForAnyArgs(compilationResult);
        }

        private void SetupCompilationWithWarning()
        {
            var errorMessage = new CompilationMessage(
                CompilerMessageId.EnumMustHaveAName, CompilerMessageType.Warning, 1, 1, 1);
            var compilationResult = new CompilationResult(
                new ThriftDocument(new List<EnumDefinition>()),
                new List<CompilationMessage> { errorMessage });

            this.compiler.Compile(default).ReturnsForAnyArgs(compilationResult);
        }

        private void SetupCompilationWithMessages(params CompilationMessage[] messages)
        {
            var compilationResult = new CompilationResult(
                new ThriftDocument(new List<EnumDefinition>()),
                messages);

            this.compiler.Compile(default).ReturnsForAnyArgs(compilationResult);
        }

        private FileInfo CreateTempFile()
        {
            var inputFile = new FileInfo(Path.GetTempFileName());
            this.tempFiles.Add(inputFile);

            return inputFile;
        }

        private DirectoryInfo GetTempDirectory()
        {
            var tempFile = new FileInfo(Path.GetTempFileName());
            tempFile.Delete();

            var tempDirectory = new DirectoryInfo(tempFile.FullName);
            this.tempDirectories.Add(tempDirectory);

            return tempDirectory;
        }
    }
}