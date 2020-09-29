namespace Thrift.Net.Tests.Compilation.Symbols.Struct
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using static Thrift.Net.Antlr.ThriftParser;

    public abstract class StructTests
    {
        private readonly IBinder fieldBinder = Substitute.For<IBinder>();
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();

        protected IBinder FieldBinder => this.fieldBinder;
        protected IBinderProvider BinderProvider => this.binderProvider;

        protected Struct CreateStructFromInput(string input)
        {
            var structNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.structDefinition());

            return new StructBuilder()
                .SetNode(structNode)
                .SetBinderProvider(this.BinderProvider)
                .Build();
        }

        protected Field SetupField(FieldContext fieldNode, Struct @struct, int? fieldId = null, string name = null)
        {
            var field = new FieldBuilder()
                .SetNode(fieldNode)
                .SetFieldId(fieldId)
                .SetName(name)
                .Build();

            this.binderProvider.GetBinder(fieldNode).Returns(this.fieldBinder);
            this.fieldBinder.Bind<Field>(fieldNode, @struct).Returns(field);

            return field;
        }
    }
}