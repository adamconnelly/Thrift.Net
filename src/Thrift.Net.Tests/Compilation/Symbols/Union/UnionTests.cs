namespace Thrift.Net.Tests.Compilation.Symbols.Union
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using static Thrift.Net.Antlr.ThriftParser;

    public abstract class UnionTests
    {
        private readonly IBinder fieldBinder = Substitute.For<IBinder>();
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();

        protected IBinder FieldBinder => this.fieldBinder;
        protected IBinderProvider BinderProvider => this.binderProvider;

        protected Union CreateUnionFromInput(string input)
        {
            var structNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.unionDefinition());

            return new UnionBuilder()
                .SetNode(structNode)
                .SetBinderProvider(this.BinderProvider)
                .Build();
        }

        protected Field SetupField(
            FieldContext fieldNode,
            Union @struct,
            int? fieldId = null,
            string name = null,
            FieldRequiredness requiredness = FieldRequiredness.Default)
        {
            var field = new FieldBuilder()
                .SetNode(fieldNode)
                .SetFieldId(fieldId)
                .SetRequiredness(requiredness)
                .SetName(name)
                .Build();

            this.binderProvider.GetBinder(fieldNode).Returns(this.fieldBinder);
            this.fieldBinder.Bind<Field>(fieldNode, @struct).Returns(field);

            return field;
        }
    }
}