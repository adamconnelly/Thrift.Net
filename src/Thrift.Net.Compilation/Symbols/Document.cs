namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using System.Linq;
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Describes a Thrift IDL file.
    /// </summary>
    public class Document : Symbol<DocumentContext, ISymbol>, IDocument
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Document" /> class.
        /// </summary>
        /// <param name="node">The document node.</param>
        /// <param name="parent">The parent symbol.</param>
        /// <param name="binderProvider">Used to get binders for nodes.</param>
        public Document(DocumentContext node, ISymbol parent, IBinderProvider binderProvider)
            : base(node, parent)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<INamespace> Namespaces
        {
            get
            {
                return this.Node.header()?.namespaceStatement()
                    .Select(namespaceNode => this.binderProvider
                        .GetBinder(namespaceNode)
                        .Bind<Namespace>(namespaceNode, this))
                    .ToList();
            }
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<IEnum> Enums
        {
            get
            {
                return this.Node.definitions().enumDefinition()
                    .Select(enumNode => this.binderProvider
                        .GetBinder(enumNode)
                        .Bind<Enum>(enumNode, this))
                    .ToList();
            }
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<IStruct> Structs
        {
            get
            {
                return this.Node.definitions().structDefinition()
                    .Select(structNode => this.binderProvider
                        .GetBinder(structNode)
                        .Bind<Struct>(structNode, this))
                    .ToList();
            }
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<IUnion> Unions
        {
            get
            {
                return this.Node.definitions().unionDefinition()
                    .Select(unionNode => this.binderProvider
                        .GetBinder(unionNode)
                        .Bind<IUnion>(unionNode, this))
                    .ToList();
            }
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<INamedTypeSymbol> AllTypes
        {
            get
            {
                return this.Enums.Cast<INamedTypeSymbol>()
                    .Union(this.Structs)
                    .Union(this.Unions)
                    .OrderBy(symbol => symbol.Node.SourceInterval.a)
                    .ToList();
            }
        }

        /// <inheritdoc/>
        public string CSharpNamespace
        {
            get
            {
                var @namespace = this.Namespaces
                    .LastOrDefault(n => n.HasCSharpScope);
                if (@namespace != null)
                {
                    return @namespace.NamespaceName;
                }

                return this.Namespaces
                    .FirstOrDefault(n => n.AppliesToAllTargets)?.NamespaceName;
            }
        }

        /// <inheritdoc/>
        public bool ContainsDefinitions => this.AllTypes.Any();

        /// <inheritdoc />
        protected override IReadOnlyCollection<ISymbol> Children
        {
            get
            {
                return this.Namespaces.Cast<ISymbol>()
                    .Union(this.Structs)
                    .Union(this.Unions)
                    .Union(this.Enums)
                    .ToList();
            }
        }

        /// <inheritdoc/>
        public bool IsMemberNameAlreadyDeclared(INamedSymbol member)
        {
            var parent = member.Node.Parent as DefinitionsContext;

            if (parent.children.Count <= 1)
            {
                return false;
            }

            return this.AllTypes
                .Where(sibling => sibling.Name == member.Name)
                .TakeWhile(sibling => sibling.Node != member.Node)
                .Any();
        }

        /// <inheritdoc/>
        public bool IsNamespaceForScopeAlreadyDeclared(INamespace @namespace)
        {
            return this.Namespaces
                .Where(n => n.Scope == @namespace.Scope)
                .TakeWhile(n => n.Node != @namespace.Node)
                .Any();
        }

        /// <inheritdoc/>
        public override INamedTypeSymbol ResolveType(string typeName)
        {
            return this.AllTypes.FirstOrDefault(e => e.Name == typeName);
        }

        /// <inheritdoc/>
        public override void Accept(ISymbolVisitor visitor)
        {
            visitor.VisitDocument(this);
            base.Accept(visitor);
        }
    }
}
