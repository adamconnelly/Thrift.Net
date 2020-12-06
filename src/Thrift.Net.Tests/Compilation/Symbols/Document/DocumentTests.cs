namespace Thrift.Net.Tests.Compilation.Symbols.Document
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Tests.Utility;
    using static Thrift.Net.Antlr.ThriftParser;

    public abstract class DocumentTests
    {
        private readonly IBinder memberBinder = Substitute.For<IBinder>();
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();

        protected Enum SetupMember(EnumDefinitionContext enumNode, string enumName, Document document)
        {
            var member = new EnumBuilder()
                .SetNode(enumNode)
                .SetName(enumName)
                .Build();

            this.binderProvider.GetBinder(enumNode).Returns(this.memberBinder);
            this.memberBinder.Bind<INamedSymbol>(enumNode, document).Returns(member);
            this.memberBinder.Bind<Enum>(enumNode, document).Returns(member);

            return member;
        }

        protected Struct SetupMember(StructDefinitionContext structNode, string structName, Document document)
        {
            var member = new StructBuilder()
                .SetNode(structNode)
                .SetName(structName)
                .Build();

            this.binderProvider.GetBinder(structNode).Returns(this.memberBinder);
            this.memberBinder.Bind<INamedSymbol>(structNode, document).Returns(member);
            this.memberBinder.Bind<Struct>(structNode, document).Returns(member);

            return member;
        }

        protected IUnion SetupMember(UnionDefinitionContext node, string name, Document document)
        {
            var member = new UnionBuilder()
                .SetNode(node)
                .SetName(name)
                .Build();

            this.binderProvider.GetBinder(node).Returns(this.memberBinder);
            this.memberBinder.Bind<INamedSymbol>(node, document).Returns(member);
            this.memberBinder.Bind<IUnion>(node, document).Returns(member);

            return member;
        }

        protected IException SetupMember(ExceptionDefinitionContext node, string name, Document document)
        {
            var member = new ExceptionBuilder()
                .SetNode(node)
                .SetName(name)
                .Build();

            this.binderProvider.GetBinder(node).Returns(this.memberBinder);
            this.memberBinder.Bind<INamedSymbol>(node, document).Returns(member);
            this.memberBinder.Bind<IException>(node, document).Returns(member);

            return member;
        }

        protected Namespace SetupNamespace(NamespaceStatementContext namespaceNode, string scope, string name, Document document)
        {
            var @namespace = new NamespaceBuilder()
                .SetNode(namespaceNode)
                .SetScope(scope)
                .SetNamespaceName(name)
                .Build();

            this.binderProvider.GetBinder(namespaceNode).Returns(this.memberBinder);
            this.memberBinder.Bind<Namespace>(namespaceNode, document).Returns(@namespace);

            return @namespace;
        }

        protected Document CreateDocumentFromInput(string input)
        {
            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            return new DocumentBuilder()
                .SetNode(documentNode)
                .SetBinderProvider(this.binderProvider)
                .Build();
        }
    }
}