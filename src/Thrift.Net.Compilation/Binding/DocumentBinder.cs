namespace Thrift.Net.Compilation.Binding
{
    using System.Linq;
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind <see cref="Document" /> objects from <see cref="DocumentContext" />
    /// nodes.
    /// </summary>
    public class DocumentBinder : Binder<DocumentContext, Document>
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentBinder" /> class.
        /// </summary>
        /// <param name="parent">The parent binder.</param>
        /// <param name="binderProvider">Used to get other binders.</param>
        public DocumentBinder(IBinder parent, IBinderProvider binderProvider)
            : base(parent)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc />
        protected override Document Bind(DocumentContext node, ISymbol parent)
        {
            var documentBuilder = new DocumentBuilder()
                .SetNode(node)
                .SetParent(parent)
                .SetBinderProvider(this.binderProvider);

            return documentBuilder.Build();
        }
    }
}