namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using Antlr4.Runtime.Tree;

    /// <summary>
    /// Represents the type of a field.
    /// </summary>
    public class FieldType : Symbol<IParseTree>
    {
        /// <summary>
        /// The 'bool' base type.
        /// </summary>
        public static readonly FieldType Bool = CreateBaseType("bool", "bool?");

        /// <summary>
        /// The 'byte' base type.
        /// </summary>
        public static readonly FieldType Byte = CreateBaseType("byte", "byte?");

        /// <summary>
        /// The 'i8' base type.
        /// </summary>
        public static readonly FieldType I8 = CreateBaseType("i8", "sbyte?");

        /// <summary>
        /// The 'i16' base type.
        /// </summary>
        public static readonly FieldType I16 = CreateBaseType("i16", "short?");

        /// <summary>
        /// The 'i32' base type.
        /// </summary>
        public static readonly FieldType I32 = CreateBaseType("i32", "int?");

        /// <summary>
        /// The 'i64' base type.
        /// </summary>
        public static readonly FieldType I64 = CreateBaseType("i64", "long?");

        /// <summary>
        /// The 'double' base type.
        /// </summary>
        public static readonly FieldType Double = CreateBaseType("double", "double?");

        /// <summary>
        /// The 'string' base type.
        /// </summary>
        public static readonly FieldType String = CreateBaseType("string", "string");

        /// <summary>
        /// The 'binary' base type.
        /// </summary>
        public static readonly FieldType Binary = CreateBaseType("binary", "byte[]");

        /// <summary>
        /// The 'slist' base type.
        /// </summary>
        public static readonly FieldType SList = CreateBaseType("slist", "string");

        private static readonly Dictionary<string, FieldType> BaseTypeMap =
            new Dictionary<string, FieldType>
            {
                { Bool.Name, Bool },
                { Byte.Name, Byte },
                { I8.Name, I8 },
                { I16.Name, I16 },
                { I32.Name, I32 },
                { I64.Name, I64 },
                { Double.Name, Double },
                { String.Name, String },
                { Binary.Name, Binary },
                { SList.Name, SList },
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldType" /> class.
        /// </summary>
        /// <param name="node">The node associated with the symbol.</param>
        /// <param name="name">The name of the type.</param>
        /// <param name="identifierPartsCount">
        /// The number of parts that make up the type name.
        /// </param>
        /// <param name="isResolved">
        /// Indicates whether the type has been resolved successfully or not.
        /// </param>
        /// <param name="csharpTypeName">
        /// The name of the C# type that this type represents.
        /// </param>
        public FieldType(
            IParseTree node,
            string name,
            int identifierPartsCount,
            bool isResolved,
            string csharpTypeName)
            : base(node, null)
        {
            this.Name = name;
            this.IdentifierPartsCount = identifierPartsCount;
            this.IsResolved = isResolved;
            this.CSharpTypeName = csharpTypeName;
        }

        /// <summary>
        /// Gets the list of base (built-in) types.
        /// </summary>
        public static IReadOnlyCollection<FieldType> BaseTypes => BaseTypeMap.Values;

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the type has successfully been
        /// resolved. If this is false it means the type could not be found.
        /// </summary>
        public bool IsResolved { get; }

        /// <summary>
        /// Gets the number of parts that make up the type name. This will be
        /// 1 for a simple type like `i32`, and 2 for a multi-part identifier
        /// like `Enums.UserType`.
        /// </summary>
        public int IdentifierPartsCount { get; }

        /// <summary>
        /// Gets the name of the C# type to use when generating this field.
        /// </summary>
        public string CSharpTypeName { get; }

        /// <summary>
        /// Creates a new type marked as resolved.
        /// </summary>
        /// <param name="node">The node associated with the symbol.</param>
        /// <param name="typeName">The name of the type.</param>
        /// <param name="csharpTypeName">The C# type name.</param>
        /// <returns>
        /// The type.
        /// </returns>
        public static FieldType CreateResolvedType(
            IParseTree node,
            string typeName,
            string csharpTypeName)
        {
            var nameParts = typeName.Split('.');

            return new FieldType(node, typeName, nameParts.Length, true, csharpTypeName);
        }

        /// <summary>
        /// Creates a new type marked as unresolved.
        /// </summary>
        /// <param name="node">The node associated with the symbol.</param>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>
        /// The type.
        /// </returns>
        public static FieldType CreateUnresolvedType(IParseTree node, string typeName)
        {
            var nameParts = typeName.Split('.');

            return new FieldType(node, typeName, nameParts.Length, false, null);
        }

        /// <summary>
        /// Gets the base type with the specified name.
        /// </summary>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>
        /// The base type, or null if the specified name isn't a base type.
        /// </returns>
        public static FieldType ResolveBaseType(string typeName)
        {
            if (BaseTypeMap.TryGetValue(typeName, out var fieldType))
            {
                return fieldType;
            }

            return null;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.Name;
        }

        private static FieldType CreateBaseType(string typeName, string csharpTypeName)
        {
            return CreateResolvedType(null, typeName, csharpTypeName);
        }
    }
}