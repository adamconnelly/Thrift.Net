namespace Thrift.Net.Tests.Compilation.Types.SetType
{
    using Thrift.Net.Compilation.Types;
    using Thrift.Net.Tests.Compilation.Types.CollectionType;

    public class SetCollectionTests : CollectionTypeTests
    {
        public SetCollectionTests()
            : base(SetType.ThriftTypeName, SetType.CSharpTypeName)
        {
        }

        protected override ICollectionType CreateCollectionType(IType elementType)
        {
            return new SetType(elementType);
        }
    }
}