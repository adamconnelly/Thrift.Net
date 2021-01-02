namespace Thrift.Net.Tests.Compilation
{
    using Thrift.Net.Compilation;
    using Xunit;

    public class ThriftFileTests
    {
        [Theory]
        [InlineData("User.thrift", "UserConstants")]
        [InlineData("Constants.thrift", "Constants")]
        [InlineData("constants.thrift", "Constants")]
        [InlineData("UserConstants.thrift", "UserConstants")]
        [InlineData("My.File.thrift", "My_FileConstants")]
        [InlineData("My-File.thrift", "My_FileConstants")]
        [InlineData("My+File.thrift", "My_FileConstants")]
        public void ConstantsClassName_GeneratesCorrectNameBasedOnFileName(string fileName, string expectedResult)
        {
            // Arrange
            var thriftFile = new ThriftFile(fileName, $"C:\\thrift\\input\\{fileName}.thrift", fileName, "C:\\thrift\\output");

            // Act
            var constantsClassName = thriftFile.ConstantsClassName;

            // Assert
            Assert.Equal(expectedResult, constantsClassName);
        }
    }
}