namespace Thrift.Net.Tests.Compilation.Binding.EnumMemberBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class BaseTypeBinderTests
    {
        private readonly BaseTypeBinder binder;

        public BaseTypeBinderTests()
        {
            this.binder = new BaseTypeBinder();
        }

        [Theory]
        [InlineData("bool")]
        [InlineData("byte")]
        [InlineData("i8")]
        [InlineData("i16")]
        [InlineData("i32")]
        [InlineData("i64")]
        [InlineData("double")]
        [InlineData("string")]
        [InlineData("binary")]
        [InlineData("slist")]
        public void CanBindAllBaseTypes(string baseType)
        {
            // Arrange
            var fieldTypeNode = ParserInput
                .FromString(baseType)
                .ParseInput(parser => parser.baseType());

            // Act
            var parent = Substitute.For<IField>();
            var type = this.binder.Bind<BaseType>(fieldTypeNode, parent);

            // Assert
            Assert.Equal(baseType, type.Name);
        }
    }
}