namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift map.
    /// </summary>
    public class MapType : CollectionType<MapTypeContext>, IMapType
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapType" /> class.
        /// </summary>
        /// <param name="node">The node the symbol is bound to.</param>
        /// <param name="parent">The parent symbol.</param>
        /// <param name="binderProvider">Used to get binders.</param>
        public MapType(MapTypeContext node, ISymbol parent, IBinderProvider binderProvider)
            : base(node, parent)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc/>
        public IFieldType KeyType
        {
            get
            {
                if (this.Node.keyType != null)
                {
                    return this.binderProvider
                        .GetBinder(this.Node.keyType)
                        .Bind<IFieldType>(this.Node.keyType, this);
                }

                return null;
            }
        }

        /// <inheritdoc/>
        public IFieldType ValueType
        {
            get
            {
                if (this.Node.valueType != null)
                {
                    return this.binderProvider
                        .GetBinder(this.Node.valueType)
                        .Bind<IFieldType>(this.Node.valueType, this);
                }

                return null;
            }
        }

        /// <inheritdoc/>
        protected override IReadOnlyCollection<ISymbol> Children
        {
            get
            {
                var children = new List<ISymbol>();
                if (this.KeyType != null)
                {
                    children.Add(this.KeyType);
                }

                if (this.ValueType != null)
                {
                    children.Add(this.ValueType);
                }

                return children;
            }
        }

        /// <inheritdoc/>
        public override void Accept(ISymbolVisitor visitor)
        {
            visitor.VisitMapType(this);
            base.Accept(visitor);
        }

        /// <inheritdoc/>
        protected override string GetTypeName()
        {
            if (this.KeyType != null && this.ValueType != null)
            {
                return $"System.Collections.Generic.Dictionary<{this.KeyType.CSharpRequiredTypeName}, {this.ValueType.CSharpRequiredTypeName}>";
            }

            return null;
        }
    }
}