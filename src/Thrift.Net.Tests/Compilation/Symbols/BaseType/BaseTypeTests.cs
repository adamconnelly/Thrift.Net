namespace Thrift.Net.Tests.Compilation.Symbols.BaseType
{
    using System;
    using System.Collections.Generic;
    using NSubstitute;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    using BaseType = Thrift.Net.Compilation.Symbols.BaseType;

    public class BaseTypeTests
    {
        public static IEnumerable<object[]> GetIsAssignableFromTestCases()
        {
            return new[]
            {
                // i8
                new object[] { BaseType.I8, BaseType.I8, true },
                new object[] { BaseType.I8, BaseType.I16, false },
                new object[] { BaseType.I8, BaseType.I32, false },
                new object[] { BaseType.I8, BaseType.I64, false },
                new object[] { BaseType.I8, BaseType.Double, false },
                new object[] { BaseType.I8, BaseType.String, false },
                new object[] { BaseType.I8, BaseType.Bool, false },
                new object[] { BaseType.I8, BaseType.Byte, true },
                new object[] { BaseType.I8, BaseType.Binary, false },
                new object[] { BaseType.I8, BaseType.Slist, false },

                // i16
                new object[] { BaseType.I16, BaseType.I8, true },
                new object[] { BaseType.I16, BaseType.I16, true },
                new object[] { BaseType.I16, BaseType.I32, false },
                new object[] { BaseType.I16, BaseType.I64, false },
                new object[] { BaseType.I16, BaseType.Double, false },
                new object[] { BaseType.I16, BaseType.String, false },
                new object[] { BaseType.I16, BaseType.Bool, false },
                new object[] { BaseType.I16, BaseType.Byte, false },
                new object[] { BaseType.I16, BaseType.Binary, false },
                new object[] { BaseType.I16, BaseType.Slist, false },

                // i32
                new object[] { BaseType.I32, BaseType.I8, true },
                new object[] { BaseType.I32, BaseType.I16, true },
                new object[] { BaseType.I32, BaseType.I32, true },
                new object[] { BaseType.I32, BaseType.I64, false },
                new object[] { BaseType.I32, BaseType.Double, false },
                new object[] { BaseType.I32, BaseType.String, false },
                new object[] { BaseType.I32, BaseType.Bool, false },
                new object[] { BaseType.I32, BaseType.Byte, false },
                new object[] { BaseType.I32, BaseType.Binary, false },
                new object[] { BaseType.I32, BaseType.Slist, false },

                // i64
                new object[] { BaseType.I64, BaseType.I8, true },
                new object[] { BaseType.I64, BaseType.I16, true },
                new object[] { BaseType.I64, BaseType.I32, true },
                new object[] { BaseType.I64, BaseType.I64, true },
                new object[] { BaseType.I64, BaseType.Double, false },
                new object[] { BaseType.I64, BaseType.String, false },
                new object[] { BaseType.I64, BaseType.Bool, false },
                new object[] { BaseType.I64, BaseType.Byte, false },
                new object[] { BaseType.I64, BaseType.Binary, false },
                new object[] { BaseType.I64, BaseType.Slist, false },

                // double
                new object[] { BaseType.Double, BaseType.I8, true },
                new object[] { BaseType.Double, BaseType.I16, true },
                new object[] { BaseType.Double, BaseType.I32, true },
                new object[] { BaseType.Double, BaseType.I64, true },
                new object[] { BaseType.Double, BaseType.Double, true },
                new object[] { BaseType.Double, BaseType.String, false },
                new object[] { BaseType.Double, BaseType.Bool, false },
                new object[] { BaseType.Double, BaseType.Byte, false },
                new object[] { BaseType.Double, BaseType.Binary, false },
                new object[] { BaseType.Double, BaseType.Slist, false },

                // string
                new object[] { BaseType.String, BaseType.I8, false },
                new object[] { BaseType.String, BaseType.I16, false },
                new object[] { BaseType.String, BaseType.I32, false },
                new object[] { BaseType.String, BaseType.I64, false },
                new object[] { BaseType.String, BaseType.Double, false },
                new object[] { BaseType.String, BaseType.String, true },
                new object[] { BaseType.String, BaseType.Bool, false },
                new object[] { BaseType.String, BaseType.Byte, false },
                new object[] { BaseType.String, BaseType.Binary, false },
                new object[] { BaseType.String, BaseType.Slist, false },

                // bool
                new object[] { BaseType.Bool, BaseType.I8, true },
                new object[] { BaseType.Bool, BaseType.I16, true },
                new object[] { BaseType.Bool, BaseType.I32, true },
                new object[] { BaseType.Bool, BaseType.I64, true },
                new object[] { BaseType.Bool, BaseType.Double, false },
                new object[] { BaseType.Bool, BaseType.String, false },
                new object[] { BaseType.Bool, BaseType.Bool, true },
                new object[] { BaseType.Bool, BaseType.Byte, false },
                new object[] { BaseType.Bool, BaseType.Binary, false },
                new object[] { BaseType.Bool, BaseType.Slist, false },

                // byte
                new object[] { BaseType.Byte, BaseType.I8, true },
                new object[] { BaseType.Byte, BaseType.I16, false },
                new object[] { BaseType.Byte, BaseType.I32, false },
                new object[] { BaseType.Byte, BaseType.I64, false },
                new object[] { BaseType.Byte, BaseType.Double, false },
                new object[] { BaseType.Byte, BaseType.String, false },
                new object[] { BaseType.Byte, BaseType.Bool, false },
                new object[] { BaseType.Byte, BaseType.Byte, true },
                new object[] { BaseType.Byte, BaseType.Binary, false },
                new object[] { BaseType.Byte, BaseType.Slist, false },

                // binary
                new object[] { BaseType.Binary, BaseType.I8, false },
                new object[] { BaseType.Binary, BaseType.I16, false },
                new object[] { BaseType.Binary, BaseType.I32, false },
                new object[] { BaseType.Binary, BaseType.I64, false },
                new object[] { BaseType.Binary, BaseType.Double, false },
                new object[] { BaseType.Binary, BaseType.String, false },
                new object[] { BaseType.Binary, BaseType.Bool, false },
                new object[] { BaseType.Binary, BaseType.Byte, false },
                new object[] { BaseType.Binary, BaseType.Binary, true },
                new object[] { BaseType.Binary, BaseType.Slist, false },

                // slist
                new object[] { BaseType.Slist, BaseType.I8, false },
                new object[] { BaseType.Slist, BaseType.I16, false },
                new object[] { BaseType.Slist, BaseType.I32, false },
                new object[] { BaseType.Slist, BaseType.I64, false },
                new object[] { BaseType.Slist, BaseType.Double, false },
                new object[] { BaseType.Slist, BaseType.String, false },
                new object[] { BaseType.Slist, BaseType.Bool, false },
                new object[] { BaseType.Slist, BaseType.Byte, false },
                new object[] { BaseType.Slist, BaseType.Binary, false },
                new object[] { BaseType.Slist, BaseType.Slist, true },
            };
        }

        [Theory]
        [InlineData("bool", "bool", "bool?")]
        [InlineData("byte", "sbyte", "sbyte?")]
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

        [Theory]
        [MemberData(nameof(GetIsAssignableFromTestCases))]
        public void IsAssignableFrom_ReturnsCorrectResult(BaseType targetType, BaseType expressionType, bool isAssignable)
        {
            Assert.Equal(isAssignable, targetType.IsAssignableFrom(expressionType));
        }

        [Fact]
        public void IsAssignableFrom_ExpressionTypeNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => BaseType.I8.IsAssignableFrom(null));
        }

        [Fact]
        public void IsAssignableFrom_ExpressionTypeNotBaseType_ReturnsFalse()
        {
            // Arrange
            var expressionType = Substitute.For<IFieldType>();
            expressionType.Name.Returns(BaseType.I8Name);

            // Act
            var isAssignable = BaseType.I8.IsAssignableFrom(expressionType);

            // Assert
            Assert.False(isAssignable);
        }
    }
}