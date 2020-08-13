namespace Thrift.Net.Tests.Compilation
{
    using System;
    using System.IO;
    using Thrift.Net.Compilation;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class ThriftFileProviderTests : IDisposable
    {
        private TempFileCreator fileCreator = new TempFileCreator();
        private bool disposed;

        [Fact]
        public void Create_FileProvided_SetsAbsolutePath()
        {
            // Arrange
            var provider = new ThriftFileProvider();
            var workingDirectory = this.fileCreator.CreateTempDirectory();
            var inputFile = new FileInfo(Path.Combine(workingDirectory.FullName, "test.thrift"));
            var outputDirectory = this.fileCreator.GetTempDirectory();

            // Act
            var thriftFile = provider.Create(workingDirectory, inputFile, outputDirectory);

            // Assert
            Assert.Equal(inputFile.FullName, thriftFile.FullName);
        }

        [Fact]
        public void Create_FileInRootWorkingDirectory_SetsRelativePath()
        {
            // Arrange
            var provider = new ThriftFileProvider();
            var workingDirectory = this.fileCreator.CreateTempDirectory();
            var inputFile = new FileInfo(Path.Combine(workingDirectory.FullName, "test.thrift"));
            var outputDirectory = this.fileCreator.GetTempDirectory();

            // Act
            var thriftFile = provider.Create(workingDirectory, inputFile, outputDirectory);

            // Assert
            Assert.Equal(inputFile.Name, thriftFile.RelativePath);
        }

        [Fact]
        public void Create_FileInSubDirectory_SetsRelativePath()
        {
            // Arrange
            var provider = new ThriftFileProvider();
            var workingDirectory = this.fileCreator.CreateTempDirectory();
            var subDirectory = new DirectoryInfo(Path.Combine(workingDirectory.FullName, "enums"));
            var inputFile = new FileInfo(Path.Combine(subDirectory.FullName, "test.thrift"));
            var outputDirectory = this.fileCreator.GetTempDirectory();

            // Act
            var thriftFile = provider.Create(workingDirectory, inputFile, outputDirectory);

            // Assert
            Assert.Equal(Path.Combine("enums", inputFile.Name), thriftFile.RelativePath);
        }

        [Fact]
        public void Create_FileOutsideWorkingDirectory_SetsRelativePath()
        {
            // Arrange
            var provider = new ThriftFileProvider();
            var workingDirectory = this.fileCreator.CreateTempDirectory();
            var codeDirectory = this.fileCreator.CreateTempDirectory();
            var inputFile = new FileInfo(Path.Combine(codeDirectory.FullName, "test.thrift"));
            var outputDirectory = this.fileCreator.GetTempDirectory();

            // Act
            var thriftFile = provider.Create(workingDirectory, inputFile, outputDirectory);

            // Assert
            Assert.Equal(Path.GetRelativePath(workingDirectory.FullName, inputFile.FullName), thriftFile.RelativePath);
        }

        [Fact]
        public void Create_FileInRootWorkingDirectory_SetsOutputPath()
        {
            // Arrange
            var provider = new ThriftFileProvider();
            var workingDirectory = this.fileCreator.CreateTempDirectory();
            var inputFile = new FileInfo(Path.Combine(workingDirectory.FullName, "test.thrift"));
            var outputDirectory = this.fileCreator.GetTempDirectory();

            // Act
            var thriftFile = provider.Create(workingDirectory, inputFile, outputDirectory);

            // Assert
            Assert.Equal(
                Path.Combine(outputDirectory.FullName, inputFile.Name.Replace(inputFile.Extension, ".cs")),
                thriftFile.OutputPath);
        }

        [Fact]
        public void Create_FileInSubdirectory_SetsOutputPath()
        {
            // Arrange
            var provider = new ThriftFileProvider();
            var workingDirectory = this.fileCreator.CreateTempDirectory();
            var subdirectory = new DirectoryInfo(Path.Combine(workingDirectory.FullName, "enums"));
            var inputFile = new FileInfo(Path.Combine(subdirectory.FullName, "test.thrift"));
            var outputDirectory = this.fileCreator.GetTempDirectory();

            // Act
            var thriftFile = provider.Create(workingDirectory, inputFile, outputDirectory);

            // Assert
            var targetDirectory = Path.Combine(outputDirectory.FullName, "enums");

            Assert.Equal(
                Path.Combine(targetDirectory, inputFile.Name.Replace(inputFile.Extension, ".cs")),
                thriftFile.OutputPath);
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
    }
}