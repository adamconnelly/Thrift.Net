namespace Thrift.Net.Tests.Compilation.Types.CollectionType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Types;

    /// <summary>
    /// A set of tests that should be run for any implementation of <see cref="CollectionType{TNode}" />.
    /// </summary>
    public abstract partial class CollectionTypeTests
    {
        private readonly string collectionTypeName;
        private readonly string csharpCollectionTypeName;

        public CollectionTypeTests(string collectionTypeName, string csharpCollectionTypeName)
        {
            this.collectionTypeName = collectionTypeName;
            this.csharpCollectionTypeName = csharpCollectionTypeName;
        }

        protected abstract ICollectionType CreateCollectionType(IType elementType);
    }
}