namespace Thrift.Net.Tests.Compilation.Symbols.Constant
{
    using System;
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class CSharpValueTests
    {
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();
        private readonly IBinder binder = Substitute.For<IBinder>();

        public CSharpValueTests()
        {
            this.binderProvider.GetBinder(default).ReturnsForAnyArgs(this.binder);
        }

        [Fact]
        public void IntConstant_UsesRawValue()
        {
            // Arrange
            var constant = this.CreateConstant("const i8 Test = 100", BaseType.I8);
            this.SetConstantValue(constant, BaseType.I8, "100");

            // Act
            var value = constant.CSharpValue;

            // Assert
            Assert.Equal("100", value);
        }

        [Fact]
        public void ValueNull_ThrowsException()
        {
            // Arrange
            var constant = this.CreateConstant("const i8 Test =", BaseType.I8);

            this.binder.Bind<IConstantExpression>(default, default)
                .ReturnsForAnyArgs((IConstantExpression)null);

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => constant.CSharpValue);
        }

        [Theory]
        [InlineData("-1", "false")]
        [InlineData("0", "false")]
        [InlineData("1", "true")]
        [InlineData("2", "true")]
        public void BoolConstant_ConvertsFromInt(string thriftExpression, string cSharpValue)
        {
            // Arrange
            var constant = this.CreateConstant($"const bool Test = {thriftExpression}", BaseType.Bool);

            this.SetConstantValue(constant, BaseType.I8, thriftExpression);

            // Act
            var value = constant.CSharpValue;

            // Assert
            Assert.Equal(cSharpValue, value);
        }

        [Fact]
        public void UnassignableConstantTypes_ThrowsException()
        {
            // Arrange
            var declaredType = Substitute.For<IFieldType>();
            var expressionType = Substitute.For<IFieldType>();

            declaredType.IsAssignableFrom(expressionType).Returns(false);

            var constant = this.CreateConstant($"const bool Test = 100", declaredType);

            this.SetConstantValue(constant, declaredType, "100");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => constant.CSharpValue);
        }

        private IConstant CreateConstant(string input, IFieldType fieldType)
        {
            var node = ParserInput.FromString(input)
                .ParseInput(parser => parser.constDefinition());

            var constant = new ConstantBuilder()
                .SetNode(node)
                .SetBinderProvider(this.binderProvider)
                .Build();

            this.binder.Bind<IFieldType>(node.fieldType(), constant).Returns(fieldType);

            return constant;
        }

        private void SetConstantValue(IConstant constant, IFieldType expressionType, string rawValue)
        {
            var constantValue = new ConstantExpressionBuilder()
                .SetNode(constant.Node.constExpression())
                .SetBinderProvider(this.binderProvider)
                .SetRawValue(rawValue)
                .SetType(expressionType)
                .Build();
            this.binder.Bind<IConstantExpression>(constant.Node.constExpression(), constant)
                .Returns(constantValue);
        }
    }
}