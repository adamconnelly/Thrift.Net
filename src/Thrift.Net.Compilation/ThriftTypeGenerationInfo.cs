namespace Thrift.Net.Compilation
{
    using System.Collections.Generic;
    using Thrift.Net.Compilation.Symbols;

    /// <summary>
    /// Used to provide mappings between Thrift types and their associated class
    /// and method names.
    /// </summary>
    public class ThriftTypeGenerationInfo
    {
        /// <summary>
        /// Maps between the Thrift type name and the type generation info.
        /// </summary>
        public static readonly IDictionary<string, ThriftTypeGenerationInfo> TypeMap
            = new Dictionary<string, ThriftTypeGenerationInfo>
        {
            {
                BaseType.ByteName,
                new ThriftTypeGenerationInfo(
                    "TType.Byte", "ReadByteAsync", "WriteByteAsync")
            },
            {
                BaseType.I8Name,
                new ThriftTypeGenerationInfo(
                    "TType.Byte", "ReadByteAsync", "WriteByteAsync")
            },
            {
                BaseType.I16Name,
                new ThriftTypeGenerationInfo("TType.I16", "ReadI16Async", "WriteI16Async")
            },
            {
                BaseType.I32Name,
                new ThriftTypeGenerationInfo("TType.I32", "ReadI32Async", "WriteI32Async")
            },
            {
                BaseType.I64Name,
                new ThriftTypeGenerationInfo("TType.I64", "ReadI64Async", "WriteI64Async")
            },
            {
                BaseType.BoolName,
                new ThriftTypeGenerationInfo("TType.Bool", "ReadBoolAsync", "WriteBoolAsync")
            },
            {
                BaseType.DoubleName,
                new ThriftTypeGenerationInfo("TType.Double", "ReadDoubleAsync", "WriteDoubleAsync")
            },
            {
                BaseType.StringName,
                new ThriftTypeGenerationInfo("TType.String", "ReadStringAsync", "WriteStringAsync")
            },
            {
                BaseType.BinaryName,
                new ThriftTypeGenerationInfo("TType.String", "ReadBinaryAsync", "WriteBinaryAsync")
            },
            {
                BaseType.SlistName,
                new ThriftTypeGenerationInfo("TType.String", "ReadStringAsync", "WriteStringAsync")
            },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ThriftTypeGenerationInfo" /> class.
        /// </summary>
        /// <param name="typeName">The `TType` instance for this type.</param>
        /// <param name="readMethodName">
        /// The method to use for reading a field of this type.
        /// </param>
        /// <param name="writeMethodName">
        /// The method to use for writing a field of this type.
        /// </param>
        public ThriftTypeGenerationInfo(string typeName, string readMethodName, string writeMethodName)
        {
            this.TypeName = typeName;
            this.ReadMethodName = readMethodName;
            this.WriteMethodName = writeMethodName;
        }

        /// <summary>
        /// Gets the name of the `TType` instance for this type.
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// Gets the name of the method to use for reading a field of this type.
        /// </summary>
        public string ReadMethodName { get; }

        /// <summary>
        /// Gets the name of the method to use for writing a field of this type.
        /// </summary>
        public string WriteMethodName { get; }
    }
}