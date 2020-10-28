namespace Thrift.Net.Compilation.Symbols
{
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift list.
    /// </summary>
    public class ListType : CollectionType<ListTypeContext>, IListType
    {
        /// <summary>
        /// The Thrift type name for a list.
        /// </summary>
        public const string ThriftTypeName = "list";

        /// <summary>
        /// The C# collection type used to represent a Thrift list.
        /// </summary>
        public const string CSharpTypeName = "System.Collections.Generic.List";

        /// <summary>
        /// Initializes a new instance of the <see cref="ListType" /> class.
        /// </summary>
        /// <param name="node">The node the symbol was created from.</param>
        /// <param name="parent">The parent of this type.</param>
        /// <param name="binderProvider">Used to get binders.</param>
        public ListType(ListTypeContext node, ISymbol parent, IBinderProvider binderProvider)
            : base(node, parent, binderProvider, ThriftTypeName, CSharpTypeName)
        {
        }

        /// <inheritdoc/>
        public override void Accept(ISymbolVisitor visitor)
        {
            visitor.VisitListType(this);
            base.Accept(visitor);
        }

        /// <inheritdoc/>
        protected override IParseTree GetElementNode()
        {
            return this.Node.fieldType();
        }
    }
}