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
        private readonly IBinder enumBinder = Substitute.For<IBinder>();
        private readonly IBinder structBinder = Substitute.For<IBinder>();
        private readonly IBinder namespaceBinder = Substitute.For<IBinder>();
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();

        public IBinder EnumBinder => this.enumBinder;
        public IBinder StructBinder => this.structBinder;
        public IBinder NamespaceBinder => this.namespaceBinder;
        public IBinderProvider BinderProvider => this.binderProvider;

        protected Enum SetupMember(EnumDefinitionContext enumNode, string enumName, Document document)
        {
            var member = new EnumBuilder()
                .SetNode(enumNode)
                .SetName(enumName)
                .Build();

            this.BinderProvider.GetBinder(enumNode).Returns(this.enumBinder);
            this.EnumBinder.Bind<INamedSymbol>(enumNode, document).Returns(member);
            this.EnumBinder.Bind<Enum>(enumNode, document).Returns(member);

            return member;
        }

        protected Struct SetupMember(StructDefinitionContext structNode, string structName, Document document)
        {
            var member = new StructBuilder()
                .SetNode(structNode)
                .SetName(structName)
                .Build();

            this.BinderProvider.GetBinder(structNode).Returns(this.structBinder);
            this.StructBinder.Bind<INamedSymbol>(structNode, document).Returns(member);
            this.StructBinder.Bind<Struct>(structNode, document).Returns(member);

            return member;
        }

        protected Namespace SetupNamespace(NamespaceStatementContext namespaceNode, string scope, string name, Document document)
        {
            var @namespace = new NamespaceBuilder()
                .SetNode(namespaceNode)
                .SetScope(scope)
                .SetNamespaceName(name)
                .Build();

            this.BinderProvider.GetBinder(namespaceNode).Returns(this.namespaceBinder);
            this.NamespaceBinder.Bind<Namespace>(namespaceNode, document).Returns(@namespace);

            return @namespace;
        }

        protected Document CreateDocumentFromInput(string input)
        {
            var documentNode = ParserInput
                .FromString(input)
                .ParseInput(parser => parser.document());

            return new DocumentBuilder()
                .SetNode(documentNode)
                .SetBinderProvider(this.BinderProvider)
                .Build();
        }
    }
}