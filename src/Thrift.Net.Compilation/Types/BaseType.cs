namespace Thrift.Net.Compilation.Types
{
    using System;
    using System.Collections.Generic;
    using Thrift.Net.Compilation.Symbols;

    /// <summary>
    /// Represents a Thrift Base type (string, bool, i32, etc).
    /// </summary>
    public class BaseType : IBaseType
    {
        /// <summary>
        /// The bool base type.
        /// </summary>
        public static readonly BaseType Bool = new BaseType("bool", "bool", "bool?");

        /// <summary>
        /// The byte base type.
        /// </summary>
        public static readonly BaseType Byte = new BaseType("byte", "byte", "byte?");

        /// <summary>
        /// The i8 base type.
        /// </summary>
        public static readonly BaseType I8 = new BaseType("i8", "sbyte", "sbyte?");

        /// <summary>
        /// The i16 base type.
        /// </summary>
        public static readonly BaseType I16 = new BaseType("i16", "short", "short?");

        /// <summary>
        /// The i32 base type.
        /// </summary>
        public static readonly BaseType I32 = new BaseType("i32", "int", "int?");

        /// <summary>
        /// The i64 base type.
        /// </summary>
        public static readonly BaseType I64 = new BaseType("i64", "long", "long?");

        /// <summary>
        /// The double base type.
        /// </summary>
        public static readonly BaseType Double = new BaseType("double", "double", "double?");

        /// <summary>
        /// The string base type.
        /// </summary>
        public static readonly BaseType String = new BaseType("string", "string", "string");

        /// <summary>
        /// The binary base type.
        /// </summary>
        public static readonly BaseType Binary = new BaseType("binary", "byte[]", "byte[]");

        /// <summary>
        /// The slist base type.
        /// </summary>
        public static readonly BaseType Slist = new BaseType("slist", "string", "string");

        /// <summary>
        /// The list of all base type names.
        /// </summary>
        public static readonly IReadOnlyCollection<string> Names = new List<string>
        {
            Byte.Name,
            I8.Name,
            I16.Name,
            I32.Name,
            I64.Name,
            Bool.Name,
            Double.Name,
            String.Name,
            Binary.Name,
            Slist.Name,
        };

        private static readonly Dictionary<string, IBaseType> TypeMap;

        static BaseType()
        {
            TypeMap = new Dictionary<string, IBaseType>
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
                { Slist.Name, Slist },
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseType" /> class.
        /// </summary>
        /// <param name="name">The name of the type.</param>
        /// <param name="requiredTypeName">The C# type name to use for required fields.</param>
        /// <param name="optionalTypeName">The C# type name to use for optional fields.</param>
        /// <remarks>
        /// BaseType doesn't have a public constructor to ensure that there's only
        /// one instance of each base type.
        /// </remarks>
        private BaseType(
            string name,
            string requiredTypeName,
            string optionalTypeName)
        {
            this.Name = name;
            this.CSharpRequiredTypeName = requiredTypeName;
            this.CSharpOptionalTypeName = optionalTypeName;
        }

        /// <summary>
        /// Gets all the base types.
        /// </summary>
        public static IReadOnlyCollection<IBaseType> All => TypeMap.Values;

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

        /// <inheritdoc/>
        public INamedTypeSymbol Definition => null;

        /// <summary>
        /// Finds the base type with the specified name.
        /// </summary>
        /// <param name="typeName">The name of the base type.</param>
        /// <param name="baseType">The base type.</param>
        /// <returns>True if <paramref name="typeName" /> is a base type, false otherwise.</returns>
        public static bool TryResolve(string typeName, out IBaseType baseType)
        {
            if (TypeMap.TryGetValue(typeName, out baseType))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool IsAssignableFrom(IType expressionType)
        {
            if (expressionType == null)
            {
                throw new ArgumentNullException(nameof(expressionType));
            }

            if (!(expressionType is IBaseType))
            {
                return false;
            }

            if (this == expressionType)
            {
                return true;
            }

            // NOTE: At the moment we're checking the type names. Once we separate
            // out the concept of a type vs a type reference, and we only have a single
            // instance of each base type, we can switch to using reference equality instead.
            if (this == I8 &&
                (expressionType == I8 ||
                expressionType == Byte))
            {
                return true;
            }

            if (this == I16 &&
                (expressionType == I8 ||
                expressionType == I16))
            {
                return true;
            }

            if (this == I32 &&
                (expressionType == I8 ||
                expressionType == I16 ||
                expressionType == I32))
            {
                return true;
            }

            if (this == I64 &&
                (expressionType == I8 ||
                expressionType == I16 ||
                expressionType == I32 ||
                expressionType == I64))
            {
                return true;
            }

            if (this == Double &&
                (expressionType == I8 ||
                expressionType == I16 ||
                expressionType == I32 ||
                expressionType == I64))
            {
                return true;
            }

            // `byte` has been superceded by the `i8` type and is equivalent, so an `i8` can
            // be assigned to a `byte`.
            if (this == Byte &&
                (expressionType == I8 ||
                expressionType == Byte))
            {
                return true;
            }

            if (this == Bool &&
                (expressionType == Bool ||
                expressionType == I8 ||
                expressionType == I16 ||
                expressionType == I32 ||
                expressionType == I64))
            {
                return true;
            }

            return false;
        }
    }
}