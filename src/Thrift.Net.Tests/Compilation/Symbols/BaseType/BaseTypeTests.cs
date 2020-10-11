namespace Thrift.Net.Tests.Compilation.Symbols.BaseType
{
    using System;
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    using BaseType = Thrift.Net.Compilation.Symbols.BaseType;

    public class BaseTypeTests
    {
        [Theory]
        [InlineData("bool", "bool", "bool?")]
        [InlineData("byte", "byte", "byte?")]
        [InlineData("i8", "sbyte", "sbyte?")]
        [InlineData("i16", "short", "short?")]
        [InlineData("i32", "int", "int?")]
        [InlineData("i64", "long", "long?")]
        [InlineData("double", "double", "double?")]
        [InlineData("string", "string", "string")]
        [InlineData("binary", "byte[]", "byte[]")]
        [InlineData("slist", "string", "string")]
        public void CanResolveAllBaseTypes(string baseType, string requiredType, string optionalType)
        {
            // Arrange
            var node = ParserInput
                .FromString(baseType)
                .ParseInput(parser => parser.baseType());
            var parent = Substitute.For<IField>();

            // Act
            var type = BaseType.Resolve(node, parent);

            // Assert
            Assert.Equal(baseType, type.Name);
            Assert.Equal(requiredType, type.CSharpRequiredTypeName);
            Assert.Equal(optionalType, type.CSharpOptionalTypeName);
        }

        [Fact]
        public void TypeNotABaseType_ThrowsException()
        {
            // Arrange
            var node = ParserInput
                .FromString("UserType")
                .ParseInput(parser => parser.baseType());
            var parent = Substitute.For<IField>();

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => BaseType.Resolve(node, parent));
        }
    }
}