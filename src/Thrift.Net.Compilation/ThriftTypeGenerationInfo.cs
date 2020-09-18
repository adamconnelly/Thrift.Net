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
                FieldType.Byte.Name,
                new ThriftTypeGenerationInfo(
                    "TType.Byte", "ReadByteAsync", "WriteByteAsync")
            },
            {
                FieldType.I8.Name,
                new ThriftTypeGenerationInfo(
                    "TType.Byte", "ReadByteAsync", "WriteByteAsync")
            },
            {
                FieldType.I16.Name,
                new ThriftTypeGenerationInfo("TType.I16", "ReadI16Async", "WriteI16Async")
            },
            {
                FieldType.I32.Name,
                new ThriftTypeGenerationInfo("TType.I32", "ReadI32Async", "WriteI32Async")
            },
            {
                FieldType.I64.Name,
                new ThriftTypeGenerationInfo("TType.I64", "ReadI64Async", "WriteI64Async")
            },
            {
                FieldType.Bool.Name,
                new ThriftTypeGenerationInfo("TType.Bool", "ReadBoolAsync", "WriteBoolAsync")
            },
            {
                FieldType.Double.Name,
                new ThriftTypeGenerationInfo("TType.Double", "ReadDoubleAsync", "WriteDoubleAsync")
            },
            {
                FieldType.String.Name,
                new ThriftTypeGenerationInfo("TType.String", "ReadStringAsync", "WriteStringAsync")
            },
            {
                FieldType.Binary.Name,
                new ThriftTypeGenerationInfo("TType.String", "ReadBinaryAsync", "WriteBinaryAsync")
            },
            {
                FieldType.SList.Name,
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