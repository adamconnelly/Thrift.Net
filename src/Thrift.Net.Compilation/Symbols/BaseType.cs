namespace Thrift.Net.Compilation.Symbols
{
    using System;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift Base type (string, bool, i32, etc).
    /// </summary>
    public class BaseType : Symbol<BaseTypeContext, IField>, IFieldType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseType" /> class.
        /// </summary>
        /// <param name="context">The node this symbol is being created from.</param>
        /// <param name="field">The field that this is the type for.</param>
        /// <param name="name">The name of the type.</param>
        /// <param name="requiredTypeName">The C# type name to use for required fields.</param>
        /// <param name="optionalTypeName">The C# type name to use for optional fields.</param>
        public BaseType(
            BaseTypeContext context,
            IField field,
            string name,
            string requiredTypeName,
            string optionalTypeName)
            : base(context, field)
        {
            this.Name = name;
            this.CSharpRequiredTypeName = requiredTypeName;
            this.CSharpOptionalTypeName = optionalTypeName;
        }

        /// <summary>
        /// Gets the name of the Thrift base type (i.e. name used in the IDL file).
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the C# type name to use for required fields.
        /// </summary>
        public string CSharpRequiredTypeName { get; }

        /// <summary>
        /// Gets the C# type name to use for optional fields.
        /// </summary>
        public string CSharpOptionalTypeName { get; }

        /// <inheritdoc />
        public bool IsResolved => true;

        /// <inheritdoc />
        public bool IsBaseType => true;

        /// <inheritdoc />
        public bool IsStruct => false;

        /// <inheritdoc />
        public bool IsEnum => false;

        /// <summary>
        /// Resolves the specified node into a base type.
        /// </summary>
        /// <param name="node">The node the type was created from.</param>
        /// <param name="parent">The field that the type definition belongs to.</param>
        /// <returns>The resolved base type.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the specified type is unknown.
        /// </exception>
        public static BaseType Resolve(BaseTypeContext node, IField parent)
        {
            return node.typeName.Text switch
            {
                "bool" => new BaseType(node, parent, "bool", "bool", "bool?"),
                "byte" => new BaseType(node, parent, "byte", "byte", "byte?"),
                "i8" => new BaseType(node, parent, "i8", "sbyte", "sbyte?"),
                "i16" => new BaseType(node, parent, "i16", "short", "short?"),
                "i32" => new BaseType(node, parent, "i32", "int", "int?"),
                "i64" => new BaseType(node, parent, "i64", "long", "long?"),
                "double" => new BaseType(node, parent, "double", "double", "double?"),
                "string" => new BaseType(node, parent, "string", "string", "string"),
                "binary" => new BaseType(node, parent, "binary", "byte[]", "byte[]"),
                "slist" => new BaseType(node, parent, "slist", "string", "string"),
                _ => throw new InvalidOperationException($"'{node.typeName.Text}' is not a known base type"),
            };
        }
    }
}