namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using System.Linq;
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// A base class for <see cref="IFieldContainer" /> objects.
    /// </summary>
    /// <typeparam name="TNode">The type of the parse tree node.</typeparam>
    /// <typeparam name="TParent">The type of the parent of this symbol.</typeparam>
    public abstract class FieldContainer<TNode, TParent> : NamedSymbol<TNode, TParent>, IFieldContainer
        where TNode : IParseTree
        where TParent : ISymbol
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldContainer{TNode, TParent}" /> class.
        /// </summary>
        /// <param name="node">The node this symbol represents.</param>
        /// <param name="parent">The parent symbol.</param>
        /// <param name="name">The name of the symbol.</param>
        protected FieldContainer(TNode node, TParent parent, string name)
            : base(node, parent, name)
        {
        }

        /// <inheritdoc/>
        public abstract IReadOnlyCollection<Field> Fields { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<Field> OptionalFields => this.Fields
            .Where(field => field.Requiredness != FieldRequiredness.Required)
            .ToList();

        /// <inheritdoc/>
        public IReadOnlyCollection<Field> RequiredFields => this.Fields
            .Where(field => field.Requiredness == FieldRequiredness.Required)
            .ToList();
    }
}