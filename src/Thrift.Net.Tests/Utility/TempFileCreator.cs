namespace Thrift.Net.Tests.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class TempFileCreator : IDisposable
    {
        private readonly List<FileInfo> tempFiles = new List<FileInfo>();
        private readonly List<DirectoryInfo> tempDirectories = new List<DirectoryInfo>();
        private bool disposed;

        /// <summary>
        /// Creates a new file in the temp folder.
        /// </summary>
        /// <returns>The information about the temp file that was created.</summary>
        public FileInfo CreateTempFile()
        {
            var inputFile = new FileInfo(Path.GetTempFileName());
            this.tempFiles.Add(inputFile);

            return inputFile;
        }

        /// <summary>
        /// Creates a new directory in the temp folder.
        /// </summary>
        /// <returns>The information about the directory that was created.</returns>
        public DirectoryInfo CreateTempDirectory()
        {
            var directory = this.GetTempDirectory();
            directory.Create();
            directory.Refresh();

            return directory;
        }

        /// <summary>
        /// Gets a unique path in the temp folder, but does not create the directory.
        /// </summary>
        /// <returns>The information about the temp directory.</summary>
        public DirectoryInfo GetTempDirectory()
        {
            var tempFile = new FileInfo(Path.GetTempFileName());
            tempFile.Delete();

            var tempDirectory = new DirectoryInfo(tempFile.FullName);
            this.tempDirectories.Add(tempDirectory);

            return tempDirectory;
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.DisposeTempFiles();
                    this.DisposeTempDirectories();
                }

                this.disposed = true;
            }
        }

        private void DisposeTempDirectories()
        {
            // Refresh temp directories to make sure the Exists check
            // is up to date.
            this.tempDirectories.ForEach(directory => directory.Refresh());

            // Delete any temp files and directories that still exist. It's not
            // the end of the world if this fails since we're creating the files
            // in the temp folder, but we want to try to avoid filling it up
            // unnecessarily.
            this.tempDirectories
                .Where(directory => directory.Exists)
                .ToList()
                .ForEach(directory => directory.Delete(true));
        }

        private void DisposeTempFiles()
        {
            // Refresh temp files to make sure the Exists check is up to date.
            this.tempFiles.ForEach(file => file.Refresh());

            // Delete any temp files that still exist. It's not
            // the end of the world if this fails since we're creating the files
            // in the temp folder, but we want to try to avoid filling it up
            // unnecessarily.
            this.tempFiles
                .Where(file => file.Exists)
                .ToList()
                .ForEach(file => file.Delete());
        }
    }
}