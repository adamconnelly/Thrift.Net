namespace Thrift.Net.Compilation.Symbols
{
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift map.
    /// </summary>
    public class MapType : Symbol<MapTypeContext, ISymbol>, IMapType
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
        public string Name => this.Node.GetText();

        /// <inheritdoc/>
        public bool IsResolved => true;

        /// <inheritdoc/>
        public string CSharpOptionalTypeName => this.GetCSharpTypeName();

        /// <inheritdoc/>
        public string CSharpRequiredTypeName => this.GetCSharpTypeName();

        /// <inheritdoc/>
        public bool IsBaseType => false;

        /// <inheritdoc/>
        public bool IsStruct => false;

        /// <inheritdoc/>
        public bool IsEnum => false;

        /// <inheritdoc/>
        public bool IsList => false;

        /// <inheritdoc/>
        public bool IsCollection => true;

        /// <inheritdoc/>
        public bool IsSet => false;

        /// <inheritdoc/>
        public bool IsMap => true;

        /// <inheritdoc/>
        public int? NestingDepth
        {
            // TODO: Add tests / rejig the inheritance chain so this can use CollectionType
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

        private string GetCSharpTypeName()
        {
            if (this.KeyType != null && this.ValueType != null)
            {
                return $"System.Collections.Generic.Dictionary<{this.KeyType.CSharpRequiredTypeName}, {this.ValueType.CSharpRequiredTypeName}>";
            }

            return null;
        }
    }
}