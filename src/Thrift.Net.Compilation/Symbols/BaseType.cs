namespace Thrift.Net.Compilation.Symbols
{
    using System;
    using System.Collections.Generic;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift Base type (string, bool, i32, etc).
    /// </summary>
    public class BaseType : Symbol<BaseTypeContext, ISymbol>, IBaseType
    {
        /// <summary>
        /// The name of the byte type.
        /// </summary>
        public const string Byte = "byte";

        /// <summary>
        /// The name of the i8 type.
        /// </summary>
        public const string I8 = "i8";

        /// <summary>
        /// The name of the i16 type.
        /// </summary>
        public const string I16 = "i16";

        /// <summary>
        /// The name of the i32 type.
        /// </summary>
        public const string I32 = "i32";

        /// <summary>
        /// The name of the i64 type.
        /// </summary>
        public const string I64 = "i64";

        /// <summary>
        /// The name of the bool type.
        /// </summary>
        public const string Bool = "bool";

        /// <summary>
        /// The name of the double type.
        /// </summary>
        public const string Double = "double";

        /// <summary>
        /// The name of the string type.
        /// </summary>
        public const string String = "string";

        /// <summary>
        /// The name of the binary type.
        /// </summary>
        public const string Binary = "binary";

        /// <summary>
        /// The name of the slist type.
        /// </summary>
        public const string Slist = "slist";

        /// <summary>
        /// The list of all base type names.
        /// </summary>
        public static readonly IReadOnlyCollection<string> Names = new List<string>
        {
            Byte,
            I8,
            I16,
            I32,
            I64,
            Bool,
            Double,
            String,
            Binary,
            Slist,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseType" /> class.
        /// </summary>
        /// <param name="context">The node this symbol is being created from.</param>
        /// <param name="parent">The parent symbol.</param>
        /// <param name="name">The name of the type.</param>
        /// <param name="requiredTypeName">The C# type name to use for required fields.</param>
        /// <param name="optionalTypeName">The C# type name to use for optional fields.</param>
        public BaseType(
            BaseTypeContext context,
            ISymbol parent,
            string name,
            string requiredTypeName,
            string optionalTypeName)
            : base(context, parent)
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

        /// <inheritdoc/>
        public bool IsList => false;

        /// <summary>
        /// Resolves the specified node into a base type.
        /// </summary>
        /// <param name="node">The node the type was created from.</param>
        /// <param name="parent">The field that the type definition belongs to.</param>
        /// <returns>The resolved base type.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the specified type is unknown.
        /// </exception>
        public static BaseType Resolve(BaseTypeContext node, ISymbol parent)
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