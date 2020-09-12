namespace Thrift.Net.Tests.Compilation.Binding.StructBinder
{
    using System;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class GetAutomaticFieldIdTests : StructBinderTests
    {
        [Fact]
        public void SingleField_ReturnsMinusOne()
        {
            // Arrange
            var input =
@"struct User {
    i32 Id
}";
            var structDefinition = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            // Act
            var fieldId = this.Binder.GetAutomaticFieldId(structDefinition.field()[0]);

            // Assert
            Assert.Equal(-1, fieldId);
        }

        [Fact]
        public void SecondField_ReturnsMinusTwo()
        {
            // Arrange
            var input =
@"struct User {
    i32 Id
    string Username
}";
            var structDefinition = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            // Act
            var fieldId = this.Binder.GetAutomaticFieldId(structDefinition.field()[1]);

            // Assert
            Assert.Equal(-2, fieldId);
        }

        [Fact]
        public void ContainsFieldsWithExplicitIds_IgnoresExplicitFields()
        {
            // Arrange
            var input =
@"struct User {
    i32 Id
    1: string CreatedOn
    string Username
}";
            var structDefinition = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            // Act
            var fieldId = this.Binder.GetAutomaticFieldId(structDefinition.field()[2]);

            // Assert
            Assert.Equal(-2, fieldId);
        }

        [Fact]
        public void FieldHasExplicitId_ThrowsException()
        {
            // Arrange
            var input =
@"struct User {
    1: string CreatedOn
}";
            var structDefinition = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => this.Binder.GetAutomaticFieldId(structDefinition.field()[0]));
        }
    }
}