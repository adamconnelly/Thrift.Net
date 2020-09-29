namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using System.Linq;
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Describes a Thrift IDL file.
    /// </summary>
    public class Document : Symbol<DocumentContext>
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

        /// <summary>
        /// Gets the C# namespace of the document.
        /// </summary>
        public IReadOnlyCollection<Namespace> Namespaces
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

        /// <summary>
        /// Gets any enums that have been defined.
        /// </summary>
        public IReadOnlyCollection<Enum> Enums
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

        /// <summary>
        /// Gets any structs that have been defined.
        /// </summary>
        public IReadOnlyCollection<Struct> Structs
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

        /// <summary>
        /// Gets all the types contained by this document, in the order they
        /// appeared in the source.
        /// </summary>
        public IReadOnlyCollection<INamedSymbol> AllTypes
        {
            get
            {
                // TODO: Test this to make sure it returns all the symbols in order
                return this.Enums.Cast<INamedSymbol>()
                    .Union(this.Structs)
                    .OrderBy(symbol => symbol.Node.SourceInterval.a)
                    .ToList();
            }
        }

        /// <summary>
        /// Gets the C# namespace that should be used for generating this document.
        /// </summary>
        /// <remarks>
        /// If multiple valid namespace scopes are provided, the compiler will
        /// use the most specific namespace and fallback to the namespace with the
        /// `*` scope if no valid C# scopes are specified. If multiple C# scopes
        /// are specified (e.g. `csharp` and `netstd`), the last one defined in
        /// the document will be used.
        /// </remarks>
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

        /// <inheritdoc />
        protected override IReadOnlyCollection<ISymbol> Children
        {
            get
            {
                return this.Namespaces.Cast<ISymbol>()
                    .Union(this.Structs)
                    .Union(this.Enums)
                    .ToList();
            }
        }

        /// <summary>
        /// Checks whether another member with the specified name has already
        /// been declared in the document.
        /// </summary>
        /// <param name="memberName">The member's name.</param>
        /// <param name="memberNode">The node being declared.</param>
        /// <returns>
        /// true if another member with the same name has been declared before
        /// <paramref name="memberNode" />. false otherwise.
        /// </returns>
        public bool IsMemberNameAlreadyDeclared(string memberName, IParseTree memberNode)
        {
            // TODO: Change parameter to `INamedSymbol`.
            var parent = memberNode.Parent as DefinitionsContext;

            if (parent.children.Count <= 1)
            {
                return false;
            }

            return this.AllTypes
                .Where(sibling => sibling.Name == memberName)
                .TakeWhile(sibling => sibling.Node != memberNode)
                .Any();
        }

        /// <summary>
        /// Checks whether another namespace statement has already been declared
        /// for the same scope.
        /// </summary>
        /// <param name="namespace">The namespace being declared.</param>
        /// <returns>
        /// true if another namespace statement has already been declared with
        /// the same scope. false otherwise.
        /// </returns>
        public bool IsNamespaceForScopeAlreadyDeclared(Namespace @namespace)
        {
            return this.Namespaces
                .Where(n => n.Scope == @namespace.Scope)
                .TakeWhile(n => n.Node != @namespace.Node)
                .Any();
        }
    }
}
