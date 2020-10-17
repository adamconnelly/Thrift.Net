namespace Thrift.Net.Compilation.Symbols
{
    using System;
    using System.Collections.Generic;
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift list.
    /// </summary>
    public class ListType : Symbol<ListTypeContext, ISymbol>, IListType
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListType" /> class.
        /// </summary>
        /// <param name="node">The node the symbol was created from.</param>
        /// <param name="parent">The parent of this type.</param>
        /// <param name="binderProvider">Used to get binders.</param>
        public ListType(ListTypeContext node, ISymbol parent, IBinderProvider binderProvider)
            : base(node, parent)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc/>
        public IFieldType ElementType
        {
            get
            {
                if (this.Node.fieldType() != null)
                {
                    return this.binderProvider
                        .GetBinder(this.Node.fieldType())
                        .Bind<IFieldType>(this.Node.fieldType(), this);
                }

                return null;
            }
        }

        /// <inheritdoc/>
        public string Name => $"list<{this.ElementType?.Name}>";

        /// <inheritdoc/>
        public bool IsResolved => true;

        /// <inheritdoc/>
        public string CSharpOptionalTypeName => this.GetTypeName();

        /// <inheritdoc/>
        public string CSharpRequiredTypeName => this.GetTypeName();

        /// <inheritdoc/>
        public bool IsBaseType => false;

        /// <inheritdoc/>
        public bool IsStruct => false;

        /// <inheritdoc/>
        public bool IsEnum => false;

        /// <inheritdoc/>
        public bool IsList => true;

        /// <summary>
        /// Gets the level of nesting of this list. A top level list returns null,
        /// and each inner list type returns an increasing number starting at 1 to
        /// indicate how deeply nested the list is.
        /// </summary>
        public int? NestingDepth
        {
            get
            {
                if (this.Parent is IListType)
                {
                    var depth = 1;
                    var parent = this.Parent;
                    while (parent.Parent is IListType)
                    {
                        parent = parent.Parent;
                        depth++;
                    }

                    return depth;
                }

                return null;
            }
        }

        /// <inheritdoc/>
        protected override IReadOnlyCollection<ISymbol> Children =>
            this.ElementType != null ? new List<ISymbol> { this.ElementType } : new List<ISymbol>();

        /// <inheritdoc/>
        public override void Accept(ISymbolVisitor visitor)
        {
            visitor.VisitListType(this);
            base.Accept(visitor);
        }

        private string GetTypeName()
        {
            if (this.ElementType != null)
            {
                return $"System.Collections.Generic.List<{this.ElementType.CSharpRequiredTypeName}>";
            }

            throw new InvalidOperationException("Cannot get the type name because no element type was provided.");
        }
    }
}