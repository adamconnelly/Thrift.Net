namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind <see cref="ListType" /> objects from <see cref="ListTypeContext" />
    /// nodes.
    /// </summary>
    public class ListTypeBinder : Binder<ListTypeContext, ListType, ISymbol>
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListTypeBinder" /> class.
        /// </summary>
        /// <param name="binderProvider">Used to get binders.</param>
        public ListTypeBinder(IBinderProvider binderProvider)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc/>
        protected override ListType Bind(ListTypeContext node, ISymbol parent)
        {
            return new ListType(node, parent, this.binderProvider);
        }
    }
}