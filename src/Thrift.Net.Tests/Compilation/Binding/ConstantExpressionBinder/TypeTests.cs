namespace Thrift.Net.Tests.Compilation.Binding.ConstantExpressionBinder
{
    using System.Collections.Generic;
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    using ConstantExpressionBinder = Thrift.Net.Compilation.Binding.ConstantExpressionBinder;

    public class TypeTests
    {
        public static IEnumerable<object[]> GetTestCases()
        {
            return new[]
            {
                new object[] { "0", BaseType.I8 },
                new object[] { sbyte.MaxValue, BaseType.I8 },
                new object[] { "+50", BaseType.I8 },
                new object[] { "-50", BaseType.I8 },
                new object[] { sbyte.MinValue - 1, BaseType.I16 },
                new object[] { sbyte.MaxValue + 1, BaseType.I16 },
                new object[] { short.MinValue - 1, BaseType.I32 },
                new object[] { short.MaxValue + 1, BaseType.I32 },
                new object[] { (long)int.MinValue - 1, BaseType.I64 },
                new object[] { (long)int.MaxValue + 1, BaseType.I64 },
                new object[] { "\"test\"", BaseType.String },
                new object[] { "\"100\"", BaseType.String },
                new object[] { "100.5", BaseType.Double },
                new object[] { "100e2", BaseType.Double },
                new object[] { "-100e2", BaseType.Double },
                new object[] { "+100e2", BaseType.Double },
            };
        }

        [Theory]
        [MemberData(nameof(GetTestCases))]
        public void SetsCorrectTypeForValue(string input, BaseType expectedType)
        {
            // Arrange
            var binder = new ConstantExpressionBinder();
            var node = ParserInput.FromString(input)
                .ParseInput(parser => parser.constExpression());
            var parent = Substitute.For<IConstant>();

            // Act
            var value = binder.Bind<IConstantExpression>(node, parent);

            // Assert
            Assert.Same(expectedType, value.Type);
        }
    }
}