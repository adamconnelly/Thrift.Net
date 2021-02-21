namespace Thrift.Net.Compilation.Types
{
    /// <summary>
    /// Represents a Thrift list.
    /// </summary>
    public class ListType : ListOrSetType, IListType
    {
        /// <summary>
        /// The Thrift type name for a list.
        /// </summary>
        public const string ThriftTypeName = "list";

        /// <summary>
        /// The C# collection type used to represent a Thrift list.
        /// </summary>
        public const string CSharpTypeName = "System.Collections.Generic.List";

        /// <summary>
        /// Initializes a new instance of the <see cref="ListType" /> class.
        /// </summary>
        /// <param name="elementType">The type of the list's element.</param>
        public ListType(IType elementType)
            : base(CSharpTypeName, ThriftTypeName, elementType)
        {
        }
    }
}