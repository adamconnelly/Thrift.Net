namespace Thrift.Net.Tests.Compilation.Symbols.SetType
{
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Compilation.Symbols.CollectionType;
    using Thrift.Net.Tests.Utility;

    public class SetCollectionTests : CollectionTypeTests
    {
        public SetCollectionTests()
            : base(SetType.ThriftTypeName, SetType.CSharpTypeName)
        {
        }

        protected override (IParseTree elementNode, ICollectionType collectionType) ParseInput(string input)
        {
            var node = ParserInput
                .FromString("set<string>")
                .ParseInput(parser => parser.setType());

            var setType = new SetTypeBuilder()
                .SetNode(node)
                .SetParent(this.Field)
                .SetBinderProvider(this.BinderProvider)
                .Build();

            return (node.fieldType(), setType);
        }
    }
}