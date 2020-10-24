namespace Thrift.Net.Tests.Compilation.Symbols.CollectionType
{
    using Antlr4.Runtime.Tree;
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;

    /// <summary>
    /// A set of tests that should be run for any implementation of <see cref="CollectionType{TNode}" />.
    /// </summary>
    public abstract partial class CollectionTypeTests
    {
        private readonly IField field = Substitute.For<IField>();
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();
        private readonly IBinder elementBinder = Substitute.For<IBinder>();
        private readonly string collectionTypeName;
        private readonly string csharpCollectionTypeName;

        public CollectionTypeTests(string collectionTypeName, string csharpCollectionTypeName)
        {
            this.collectionTypeName = collectionTypeName;
            this.csharpCollectionTypeName = csharpCollectionTypeName;
        }

        protected IField Field => this.field;
        protected IBinderProvider BinderProvider => this.binderProvider;
        protected IBinder ElementBinder => this.elementBinder;

        protected abstract (IParseTree elementNode, ICollectionType collectionType) ParseInput(string input);
    }
}