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
        public const string ByteName = "byte";

        /// <summary>
        /// The name of the i8 type.
        /// </summary>
        public const string I8Name = "i8";

        /// <summary>
        /// The name of the i16 type.
        /// </summary>
        public const string I16Name = "i16";

        /// <summary>
        /// The name of the i32 type.
        /// </summary>
        public const string I32Name = "i32";

        /// <summary>
        /// The name of the i64 type.
        /// </summary>
        public const string I64Name = "i64";

        /// <summary>
        /// The name of the bool type.
        /// </summary>
        public const string BoolName = "bool";

        /// <summary>
        /// The name of the double type.
        /// </summary>
        public const string DoubleName = "double";

        /// <summary>
        /// The name of the string type.
        /// </summary>
        public const string StringName = "string";

        /// <summary>
        /// The name of the binary type.
        /// </summary>
        public const string BinaryName = "binary";

        /// <summary>
        /// The name of the slist type.
        /// </summary>
        public const string SlistName = "slist";

        /// <summary>
        /// The bool base type.
        /// </summary>
        public static readonly BaseType Bool = new BaseType(null, null, BoolName, "bool", "bool?");

        /// <summary>
        /// The byte base type.
        /// </summary>
        public static readonly BaseType Byte = new BaseType(null, null, ByteName, "byte", "byte?");

        /// <summary>
        /// The i8 base type.
        /// </summary>
        public static readonly BaseType I8 = new BaseType(null, null, I8Name, "sbyte", "sbyte?");

        /// <summary>
        /// The i16 base type.
        /// </summary>
        public static readonly BaseType I16 = new BaseType(null, null, I16Name, "short", "short?");

        /// <summary>
        /// The i32 base type.
        /// </summary>
        public static readonly BaseType I32 = new BaseType(null, null, I32Name, "int", "int?");

        /// <summary>
        /// The i64 base type.
        /// </summary>
        public static readonly BaseType I64 = new BaseType(null, null, I64Name, "long", "long?");

        /// <summary>
        /// The double base type.
        /// </summary>
        public static readonly BaseType Double = new BaseType(null, null, DoubleName, "double", "double?");

        /// <summary>
        /// The string base type.
        /// </summary>
        public static readonly BaseType String = new BaseType(null, null, StringName, "string", "string");

        /// <summary>
        /// The binary base type.
        /// </summary>
        public static readonly BaseType Binary = new BaseType(null, null, BinaryName, "byte[]", "byte[]");

        /// <summary>
        /// The slist base type.
        /// </summary>
        public static readonly BaseType Slist = new BaseType(null, null, SlistName, "string", "string");

        /// <summary>
        /// The list of all base type names.
        /// </summary>
        public static readonly IReadOnlyCollection<string> Names = new List<string>
        {
            ByteName,
            I8Name,
            I16Name,
            I32Name,
            I64Name,
            BoolName,
            DoubleName,
            StringName,
            BinaryName,
            SlistName,
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

        /// <inheritdoc/>
        public bool IsCollection => false;

        /// <inheritdoc/>
        public bool IsSet => false;

        /// <inheritdoc/>
        public bool IsMap => false;

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
            // TODO: Remove this method once we refactor and separate the concept of
            // types vs type references.
            return node.typeName.Text switch
            {
                BoolName => new BaseType(node, parent, BoolName, "bool", "bool?"),
                ByteName => new BaseType(node, parent, ByteName, "byte", "byte?"),
                I8Name => new BaseType(node, parent, I8Name, "sbyte", "sbyte?"),
                I16Name => new BaseType(node, parent, I16Name, "short", "short?"),
                I32Name => new BaseType(node, parent, I32Name, "int", "int?"),
                I64Name => new BaseType(node, parent, I64Name, "long", "long?"),
                DoubleName => new BaseType(node, parent, DoubleName, "double", "double?"),
                StringName => new BaseType(node, parent, StringName, "string", "string"),
                BinaryName => new BaseType(node, parent, BinaryName, "byte[]", "byte[]"),
                SlistName => new BaseType(node, parent, SlistName, "string", "string"),
                _ => throw new InvalidOperationException($"'{node.typeName.Text}' is not a known base type"),
            };
        }
    }
}