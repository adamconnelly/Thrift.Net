namespace Thrift.Net.Tests.Compilation.Binding.FieldBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using Xunit;

    public class FieldIdTests
    {
        private readonly IFieldContainerBinder containerBinder = Substitute.For<IFieldContainerBinder>();
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();
        private readonly FieldBinder binder;

        public FieldIdTests()
        {
            this.binder = new FieldBinder(this.containerBinder, this.binderProvider);
        }

        [Fact]
        public void Bind_FieldIdProvided_SetsFieldId()
        {
            // Arrange
            var @struct = new StructBuilder().Build();
            var fieldContext = ParserInput
                .FromString("1: i32 Id")
                .ParseInput(parser => parser.field());

            // Act
            var field = this.binder.Bind<Field>(fieldContext, @struct);

            // Assert
            Assert.Equal(1, field.FieldId);
        }

        [Fact]
        public void Bind_FieldIdNotProvided_GeneratesNegativeIds()
        {
            // Arrange
            var input =
@"struct User {
    i32 Id
    string Username
}";
            var @struct = new StructBuilder().Build();
            var structDefinition = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            // Act
            var idField = this.binder.Bind<Field>(structDefinition.field()[0], @struct);
            var usernameField = this.binder.Bind<Field>(structDefinition.field()[1], @struct);

            // Assert
            Assert.Equal(-1, idField.FieldId);
            Assert.Equal(-2, usernameField.FieldId);
        }

        [Fact]
        public void Bind_FieldIdNotProvided_SkipsExplicitFieldsWhenGeneratingIds()
        {
            // TODO: Figure out what to do here
            // Arrange
            var input =
@"struct User {
    i32 Id
    1: string Username
    string Email
}";
            var @struct = new StructBuilder().Build();
            var structDefinition = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            // Act
            var field = this.binder.Bind<Field>(structDefinition.field()[2], @struct);

            // Assert
            Assert.Equal(-2, field.FieldId);
        }

        [Fact]
        public void Bind_FieldIdNotProvided_IndicatesFieldIdIsImplicit()
        {
            // Arrange
            var input =
@"struct User {
    i32 Id
}";
            var @struct = new StructBuilder().Build();
            var structDefinition = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            // Act
            var field = this.binder.Bind<Field>(structDefinition.field()[0], @struct);

            // Assert
            Assert.True(field.IsFieldIdImplicit);
        }

        [Fact]
        public void Bind_FieldIdNegative_SetsFieldIdNull()
        {
            // Arrange
            var @struct = new StructBuilder().Build();
            var fieldContext = ParserInput
                .FromString("-1: i32 Id")
                .ParseInput(parser => parser.field());

            // Act
            var field = this.binder.Bind<Field>(fieldContext, @struct);

            // Assert
            Assert.Null(field.FieldId);
        }

        [Fact]
        public void Bind_FieldIdSupplied_SetsRawFieldId()
        {
            // Arrange
            var @struct = new StructBuilder().Build();
            var fieldContext = ParserInput
                .FromString("4: i32 Id")
                .ParseInput(parser => parser.field());

            // Act
            var field = this.binder.Bind<Field>(fieldContext, @struct);

            // Assert
            Assert.Equal("4", field.RawFieldId);
        }

        [Fact]
        public void Bind_FieldIdNotAnInteger_SetsFieldIdNull()
        {
            // Arrange
            var @struct = new StructBuilder().Build();
            var fieldContext = ParserInput
                .FromString("abc: i32 Id")
                .ParseInput(parser => parser.field());

            // Act
            var field = this.binder.Bind<Field>(fieldContext, @struct);

            // Assert
            Assert.Null(field.FieldId);
        }
    }
}