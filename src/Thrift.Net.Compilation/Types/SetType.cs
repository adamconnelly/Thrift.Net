namespace Thrift.Net.Compilation.Types
{
    /// <summary>
    /// Represents a Thrift set.
    /// </summary>
    public class SetType : ListOrSetType, ISetType
    {
        /// <summary>
        /// The Thrift type name for a set.
        /// </summary>
        public const string ThriftTypeName = "set";

        /// <summary>
        /// The name of the C# type to use to generate a set.
        /// </summary>
        public const string CSharpTypeName = "System.Collections.Generic.HashSet";

        /// <summary>
        /// Initializes a new instance of the <see cref="SetType" /> class.
        /// </summary>
        /// <param name="elementType">The type of element the set contains.</param>
        public SetType(IType elementType)
            : base(CSharpTypeName, ThriftTypeName, elementType)
        {
        }
    }
}