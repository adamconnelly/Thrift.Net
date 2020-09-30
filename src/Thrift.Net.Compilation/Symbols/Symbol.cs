namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using Antlr4.Runtime.Tree;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// The base class for symbols.
    /// </summary>
    /// <typeparam name="TNode">
    /// The type of the node associated with the symbol.
    /// </typeparam>
    public abstract class Symbol<TNode> : ISymbol<TNode>
        where TNode : IParseTree
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Symbol{T}" /> class.
        /// </summary>
        /// <param name="node">The node associated with this symbol.</param>
        /// <param name="parent">The parent symbol.</param>
        protected Symbol(TNode node, ISymbol parent)
        {
            this.Node = node;
            this.Parent = parent;
        }

        /// <inheritdoc />
        IParseTree ISymbol.Node => this.Node;

        /// <inheritdoc />
        public TNode Node { get; private set; }

        /// <inheritdoc />
        public ISymbol Parent { get; }

        /// <summary>
        /// Gets the child symbols. This should be overridden in any container symbols.
        /// </summary>
        protected virtual IReadOnlyCollection<ISymbol> Children => new List<ISymbol>();

        /// <inheritdoc />
        public ISymbol FindSymbolForNode(IParseTree node)
        {
            if (object.ReferenceEquals(this.Node, node))
            {
                return this;
            }

            foreach (var child in this.Children)
            {
                var childSymbol = child.FindSymbolForNode(node);
                if (childSymbol != null)
                {
                    return childSymbol;
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public Document FindSymbolForNode(DocumentContext node)
        {
            if (this.FindSymbolForNode((IParseTree)node) is Document document)
            {
                return document;
            }

            return null;
        }

        /// <inheritdoc/>
        public Enum FindSymbolForNode(EnumDefinitionContext node)
        {
            if (this.FindSymbolForNode((IParseTree)node) is Enum @enum)
            {
                return @enum;
            }

            return null;
        }

        /// <inheritdoc/>
        public EnumMember FindSymbolForNode(EnumMemberContext node)
        {
            if (this.FindSymbolForNode((IParseTree)node) is EnumMember enumMember)
            {
                return enumMember;
            }

            return null;
        }

        /// <inheritdoc/>
        public Field FindSymbolForNode(FieldContext node)
        {
            if (this.FindSymbolForNode((IParseTree)node) is Field field)
            {
                return field;
            }

            return null;
        }

        /// <inheritdoc/>
        public Namespace FindSymbolForNode(NamespaceStatementContext node)
        {
            if (this.FindSymbolForNode((IParseTree)node) is Namespace @namespace)
            {
                return @namespace;
            }

            return null;
        }

        /// <inheritdoc/>
        public Struct FindSymbolForNode(StructDefinitionContext node)
        {
            if (this.FindSymbolForNode((IParseTree)node) is Struct @struct)
            {
                return @struct;
            }

            return null;
        }

        /// <inheritdoc />
        public FieldType ResolveType(string typeName)
        {
            return this.Parent?.ResolveType(typeName);
        }
    }
}