namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using Xunit;

    using ThriftCompiler = Thrift.Net.Compilation.ThriftCompiler;

    public class EnumTests
    {
        private readonly ThriftCompiler compiler = new ThriftCompiler();

        [Fact]
        public void Compile_DocumentContainsEnum_AddsEnumToModel()
        {
            // Arrange
            var inputStream = CreateInputStream(
@"enum UserType
{
    User,
    Administrator
}");

            // Act
            var result = this.compiler.Compile(inputStream);

            // Assert
            Assert.Collection(
                result.Document.Enums,
                item => Assert.Equal("UserType", item.Name));
        }

        [Fact]
        public void Compile_DocumentContainsMultipleEnums_AddsAllEnums()
        {
            // Arrange
            var inputStream = CreateInputStream(
@"enum UserType
{
    User
}

enum Permission
{
    CanRead
}");

            // Act
            var result = this.compiler.Compile(inputStream);

            // Assert
            Assert.Collection(
                result.Document.Enums,
                item => Assert.Equal("UserType", item.Name),
                item => Assert.Equal("Permission", item.Name));
        }

        [Fact]
        public void Compile_EnumContainsMember_AddsMemberToEnum()
        {
            // Arrange
            var inputStream = CreateInputStream(
@"enum UserType
{
    User
}");

            // Act
            var result = this.compiler.Compile(inputStream);

            // Assert
            Assert.Collection(
                result.Document.Enums.Single().Members,
                item => Assert.Equal("User", item.Name));
        }

        [Fact]
        public void Compile_EnumContainsMultipleMembers_AddsMembersToEnum()
        {
            // Arrange
            var inputStream = CreateInputStream(
@"enum UserType
{
    User,
    Administrator
}");

            // Act
            var result = this.compiler.Compile(inputStream);

            // Assert
            Assert.Collection(
                result.Document.Enums.Single().Members,
                item => Assert.Equal("User", item.Name),
                item => Assert.Equal("Administrator", item.Name));
        }

        [Fact]
        public void Compile_EnumMembersHaveValuesSpecified_SetsValuesCorrectly()
        {
            // Arrange
            var inputStream = CreateInputStream(
@"enum UserType
{
    User = 1,
    Administrator = 5
}");

            // Act
            var result = this.compiler.Compile(inputStream);

            // Assert
            Assert.Collection(
                result.Document.Enums.Single().Members,
                item => Assert.Equal(1, item.Value),
                item => Assert.Equal(5, item.Value));
        }

        // TODO: enum values can automatically be generated.
        // TODO: enum value should be previous value + 1 if not specified.
        // TODO: error if a non-int enum value specified.
        // TODO: error if negative enum value specified.
        // TODO: error if enum name is missing

        private static MemoryStream CreateInputStream(string input)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(input));
        }
    }
}