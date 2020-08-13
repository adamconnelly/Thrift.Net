namespace Thrift.Net.Tests.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.IO;
    using NSubstitute;
    using Thrift.Net.Compilation;
    using Thrift.Net.Compilation.Model;
    using Thrift.Net.Compiler;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class ProgramTests : IDisposable
    {
        private readonly IConsole console = Substitute.For<IConsole>();
        private readonly IThriftCompiler compiler = Substitute.For<IThriftCompiler>();
        private readonly IThriftFileProvider fileProvider = Substitute.For<IThriftFileProvider>();
        private readonly IThriftDocumentGenerator documentGenerator = Substitute.For<IThriftDocumentGenerator>();
        private readonly IThriftFileWriter fileWriter = Substitute.For<IThriftFileWriter>();
        private TempFileCreator fileCreator = new TempFileCreator();
        private bool disposed;

        public ProgramTests()
        {
            Program.Compiler = this.compiler;
            Program.FileProvider = this.fileProvider;
            Program.DocumentGenerator = this.documentGenerator;
            Program.FileWriter = this.fileWriter;
        }

        [Fact]
        public void When_InputFileDoesNotExist_ReturnsError()
        {
            // Arrange
            var inputFile = new FileInfo("missing-file.thrift");
            var outputDirectory = this.fileCreator.GetTempDirectory();

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
            var outputDirectory = this.fileCreator.GetTempDirectory();

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
            var inputFile = this.fileCreator.CreateTempFile();
            var outputDirectory = this.fileCreator.GetTempDirectory();
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
            var inputFile = this.fileCreator.CreateTempFile();
            var outputDirectory = this.fileCreator.GetTempDirectory();
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
            var inputFile = this.fileCreator.CreateTempFile();
            var outputDirectory = this.fileCreator.GetTempDirectory();
            this.SetupCompilationWithError();
            this.SetupCreateThriftFile(inputFile);

            // Act
            var result = Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            Assert.Equal((int)ExitCode.CompilationFailed, result);
        }

        [Fact]
        public void When_CompilationHasWarnings_ReturnsSuccess()
        {
            // Arrange
            var inputFile = this.fileCreator.CreateTempFile();
            var outputDirectory = this.fileCreator.GetTempDirectory();
            this.SetupCompilationWithWarning();
            this.SetupCreateThriftFile(inputFile);

            // Act
            var result = Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            Assert.Equal((int)ExitCode.Success, result);
        }

        [Fact]
        public void When_CompilationHasMessages_OutputsMessages()
        {
            // Arrange
            var inputFile = this.fileCreator.CreateTempFile();
            var outputDirectory = this.fileCreator.GetTempDirectory();
            this.SetupCompilationWithMessages(
                new CompilationMessage(CompilerMessageId.EnumMustHaveAName, CompilerMessageType.Error, 1, 1, 4),
                new CompilationMessage(CompilerMessageId.EnumMemberMustHaveAName, CompilerMessageType.Error, 2, 4, 8));
            this.SetupCreateThriftFile(inputFile);

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
            var inputFile = this.fileCreator.CreateTempFile();
            var outputDirectory = this.fileCreator.GetTempDirectory();
            this.SetupCompilationWithMessages(
                new CompilationMessage(CompilerMessageId.EnumMustHaveAName, CompilerMessageType.Error, 1, 1, 4),
                new CompilationMessage(CompilerMessageId.EnumMemberMustHaveAName, CompilerMessageType.Error, 2, 4, 8));
            this.SetupCreateThriftFile(inputFile);

            // Act
            Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            this.console.Out.Received().Write($"Compilation failed with 2 error(s):{Environment.NewLine}{Environment.NewLine}");
        }

        [Fact]
        public void When_CompilationHasErrorsAndWarnings_OutputsErrorCount()
        {
            // Arrange
            var inputFile = this.fileCreator.CreateTempFile();
            var outputDirectory = this.fileCreator.GetTempDirectory();
            this.SetupCompilationWithMessages(
                new CompilationMessage(CompilerMessageId.EnumMustHaveAName, CompilerMessageType.Error, 1, 1, 4),
                new CompilationMessage(CompilerMessageId.EnumMemberMustHaveAName, CompilerMessageType.Error, 2, 4, 8),
                new CompilationMessage(CompilerMessageId.EnumMemberMustHaveAName, CompilerMessageType.Warning, 2, 4, 8));
            this.SetupCreateThriftFile(inputFile);

            // Act
            Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            this.console.Out.Received().Write($"Compilation failed with 2 error(s) and 1 warning(s):{Environment.NewLine}{Environment.NewLine}");
        }

        [Fact]
        public void When_CompilationHasWarnings_OutputsWarningCount()
        {
            // Arrange
            var inputFile = this.fileCreator.CreateTempFile();
            var outputDirectory = this.fileCreator.GetTempDirectory();
            this.SetupCompilationWithMessages(
                new CompilationMessage(CompilerMessageId.EnumMustHaveAName, CompilerMessageType.Warning, 1, 1, 4),
                new CompilationMessage(CompilerMessageId.EnumMemberMustHaveAName, CompilerMessageType.Warning, 2, 4, 8),
                new CompilationMessage(CompilerMessageId.EnumMemberMustHaveAName, CompilerMessageType.Warning, 2, 4, 8));
            this.SetupCreateThriftFile(inputFile);

            // Act
            Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            this.console.Out.Received().Write($"Compilation succeeded with 3 warning(s):{Environment.NewLine}{Environment.NewLine}");
        }

        [Fact]
        public void When_CompilationHasNoErrorsOrWarnings_OutputsSuccessMessage()
        {
            // Arrange
            var inputFile = this.fileCreator.CreateTempFile();
            var outputDirectory = this.fileCreator.GetTempDirectory();
            this.SetupSuccessfulCompilation();

            // Act
            Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            this.console.Out.Received().Write($"Compilation succeeded with no errors or warnings!{Environment.NewLine}");
        }

        [Fact]
        public void When_CompilationSucceeds_WritesGeneratedCode()
        {
            // Arrange
            var inputFile = this.fileCreator.CreateTempFile();
            var outputDirectory = this.fileCreator.GetTempDirectory();
            this.SetupSuccessfulCompilation();
            var thriftFile = this.SetupCreateThriftFile(inputFile);

            const string generatedCode = "namespace Thrift.Net.Tests ...";
            this.documentGenerator.Generate(default).ReturnsForAnyArgs(generatedCode);

            // Act
            Program.Main(inputFile, outputDirectory, this.console);

            // Assert
            this.fileWriter.Received().Write(thriftFile, generatedCode);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.fileCreator?.Dispose();
                }

                this.fileCreator = null;

                this.disposed = true;
            }
        }

        private void SetupSuccessfulCompilation()
        {
            var compilationResult = new CompilationResult(
                new ThriftDocument(null, new List<EnumDefinition>()),
                new List<CompilationMessage>());

            this.compiler.Compile(default).ReturnsForAnyArgs(compilationResult);
        }

        private void SetupCompilationWithError()
        {
            var errorMessage = new CompilationMessage(
                CompilerMessageId.EnumMustHaveAName, CompilerMessageType.Error, 1, 1, 1);
            var compilationResult = new CompilationResult(
                new ThriftDocument(null, new List<EnumDefinition>()),
                new List<CompilationMessage> { errorMessage });

            this.compiler.Compile(default).ReturnsForAnyArgs(compilationResult);
        }

        private void SetupCompilationWithWarning()
        {
            var errorMessage = new CompilationMessage(
                CompilerMessageId.EnumMustHaveAName, CompilerMessageType.Warning, 1, 1, 1);
            var compilationResult = new CompilationResult(
                new ThriftDocument(null, new List<EnumDefinition>()),
                new List<CompilationMessage> { errorMessage });

            this.compiler.Compile(default).ReturnsForAnyArgs(compilationResult);
        }

        private void SetupCompilationWithMessages(params CompilationMessage[] messages)
        {
            var compilationResult = new CompilationResult(
                new ThriftDocument(null, new List<EnumDefinition>()),
                messages);

            this.compiler.Compile(default).ReturnsForAnyArgs(compilationResult);
        }

        private ThriftFile SetupCreateThriftFile(FileInfo inputFile)
        {
            var thriftFile = new ThriftFile(inputFile.FullName, inputFile.Name, $"output/{inputFile.Name}");
            this.fileProvider.Create(default, default, default).ReturnsForAnyArgs(thriftFile);
            return thriftFile;
        }
    }
}