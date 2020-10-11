namespace Thrift.Net.Compilation.Symbols
{
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// Represents an unresolved type (i.e. where a type has been referenced, but
    /// no definition can be found).
    /// </summary>
    public class UnresolvedType : NamedSymbol<IParseTree, ISymbol>, IUnresolvedType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnresolvedType" /> class.
        /// </summary>
        /// <param name="node">The node representing the unresolved reference.</param>
        /// <param name="parent">The parent symbol.</param>
        /// <param name="name">The type name that has been referenced.</param>
        public UnresolvedType(IParseTree node, ISymbol parent, string name)
            : base(node, parent, name)
        {
        }

        /// <inheritdoc/>
        public IDocument Document => throw new System.NotImplementedException();
    }
}