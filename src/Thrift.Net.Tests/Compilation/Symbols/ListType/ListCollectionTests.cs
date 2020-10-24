namespace Thrift.Net.Tests.Compilation.Symbols.ListType
{
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Compilation.Symbols.CollectionType;
    using Thrift.Net.Tests.Utility;

    public class ListCollectionTests : CollectionTypeTests
    {
        public ListCollectionTests()
            : base(ListType.ThriftTypeName, ListType.CSharpTypeName)
        {
        }

        protected override (IParseTree elementNode, ICollectionType collectionType) ParseInput(string input)
        {
            var node = ParserInput
                .FromString("list<string>")
                .ParseInput(parser => parser.listType());

            var listType = new ListTypeBuilder()
                .SetNode(node)
                .SetParent(this.Field)
                .SetBinderProvider(this.BinderProvider)
                .Build();

            return (node.fieldType(), listType);
        }
    }
}