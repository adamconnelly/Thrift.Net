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
            var fieldContext = ParserInput
                .FromString("1: i32 Id")
                .ParseInput(parser => parser.field());

            // Act
            var field = this.binder.Bind<Field>(fieldContext);

            // Assert
            Assert.Equal(1, field.FieldId);
        }

        [Fact]
        public void Bind_FieldIdNotProvided_DefaultsIdToZero()
        {
            // Arrange
            var fieldContext = ParserInput
                .FromString("i32 Id")
                .ParseInput(parser => parser.field());

            // Act
            var field = this.binder.Bind<Field>(fieldContext);

            // Assert
            Assert.Equal(0, field.FieldId);
        }

        [Fact]
        public void Bind_FieldIdNotProvided_GetsFieldIdFromContainer()
        {
            // Arrange
            var previousField = new FieldBuilder()
                .SetFieldId(5)
                .Build();

            var fieldContext = ParserInput
                .FromString("i32 Id")
                .ParseInput(parser => parser.field());
            this.containerBinder.GetAutomaticFieldId(fieldContext).Returns(-3);

            // Act
            var field = this.binder.Bind<Field>(fieldContext);

            // Assert
            Assert.Equal(-3, field.FieldId);
        }

        [Fact]
        public void Bind_FieldIdNotProvided_IndicatesFieldIdIsImplicit()
        {
            // Arrange
            var previousField = new FieldBuilder()
                .SetFieldId(5)
                .Build();

            var fieldContext = ParserInput
                .FromString("i32 Id")
                .ParseInput(parser => parser.field());

            // Act
            var field = this.binder.Bind<Field>(fieldContext);

            // Assert
            Assert.True(field.IsFieldIdImplicit);
        }

        [Fact]
        public void Bind_FieldIdNegative_SetsFieldIdNull()
        {
            // Arrange
            var fieldContext = ParserInput
                .FromString("-1: i32 Id")
                .ParseInput(parser => parser.field());

            // Act
            var field = this.binder.Bind<Field>(fieldContext);

            // Assert
            Assert.Null(field.FieldId);
        }

        [Fact]
        public void Bind_FieldIdSupplied_SetsRawFieldId()
        {
            // Arrange
            var fieldContext = ParserInput
                .FromString("4: i32 Id")
                .ParseInput(parser => parser.field());

            // Act
            var field = this.binder.Bind<Field>(fieldContext);

            // Assert
            Assert.Equal("4", field.RawFieldId);
        }

        [Fact]
        public void Bind_FieldIdNotAnInteger_SetsFieldIdNull()
        {
            // Arrange
            var fieldContext = ParserInput
                .FromString("abc: i32 Id")
                .ParseInput(parser => parser.field());

            // Act
            var field = this.binder.Bind<Field>(fieldContext);

            // Assert
            Assert.Null(field.FieldId);
        }
    }
}