namespace Thrift.Net.Compilation.Symbols
{
    using System;
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift set.
    /// </summary>
    public class SetType : Symbol<SetTypeContext, ISymbol>, ISetType
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetType" /> class.
        /// </summary>
        /// <param name="node">The node this symbol is bound to.</param>
        /// <param name="parent">This symbol's parent.</param>
        /// <param name="binderProvider">Used to bind child symbols.</param>
        public SetType(SetTypeContext node, ISymbol parent, IBinderProvider binderProvider)
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
        public string Name => $"set<{this.ElementType?.Name}>";

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
        public bool IsList => false;

        /// <inheritdoc/>
        public int? NestingDepth
        {
            get
            {
                if (this.Parent is ICollectionType)
                {
                    var depth = 1;
                    var parent = this.Parent;
                    while (parent.Parent is ICollectionType)
                    {
                        parent = parent.Parent;
                        depth++;
                    }

                    return depth;
                }

                return null;
            }
        }

        private string GetTypeName()
        {
            if (this.ElementType != null)
            {
                return $"System.Collections.Generic.HashSet<{this.ElementType.CSharpRequiredTypeName}>";
            }

            throw new InvalidOperationException("Cannot get the type name because no element type was provided.");
        }
    }
}