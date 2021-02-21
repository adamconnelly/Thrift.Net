namespace Thrift.Net.Tests.Compilation.Types.ListType
{
    using NSubstitute;
    using Thrift.Net.Compilation.Types;
    using Thrift.Net.Tests.Compilation.Types.CollectionType;

    public class ListCollectionTests : CollectionTypeTests
    {
        public ListCollectionTests()
            : base(ListType.ThriftTypeName, ListType.CSharpTypeName)
        {
        }

        protected override ICollectionType CreateCollectionType(IType elementType)
        {
            return new ListType(elementType);
        }
    }
}