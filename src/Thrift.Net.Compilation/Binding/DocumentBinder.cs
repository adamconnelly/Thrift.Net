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
    public class DocumentBinder : Binder<DocumentContext, Document>, IDocumentBinder
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
        public bool IsMemberNameAlreadyDeclared(string memberName, IParseTree memberNode)
        {
            var parent = memberNode.Parent as DefinitionsContext;

            if (parent.children.Count <= 1)
            {
                return false;
            }

            return parent.children
                .Select(sibling => this.binderProvider
                    .GetBinder(sibling)
                    .Bind<INamedSymbol>(sibling))
                .Where(sibling => sibling.Name == memberName)
                .TakeWhile(sibling => sibling.Node != memberNode)
                .Any();
        }

        /// <inheritdoc />
        protected override Document Bind(DocumentContext node)
        {
            var documentBuilder = new DocumentBuilder()
                .SetNode(node)
                .AddNamespaces(node.header()?.namespaceStatement()
                    .Select(namespaceNode => this.binderProvider
                        .GetBinder(namespaceNode)
                        .Bind<Namespace>(namespaceNode)))
                .AddEnums(node.definitions().enumDefinition()
                    .Select(enumNode => this.binderProvider
                        .GetBinder(enumNode)
                        .Bind<Enum>(enumNode)))
                .AddStructs(node.definitions().structDefinition()
                    .Select(structNode => this.binderProvider
                        .GetBinder(structNode)
                        .Bind<Struct>(structNode)));

            return documentBuilder.Build();
        }
    }
}