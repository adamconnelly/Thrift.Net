namespace Thrift.Net.Compilation.Types
{
    using System;
    using Thrift.Net.Compilation.Symbols;

    /// <summary>
    /// Represents a user-defined type like an enum, struct, etc.
    /// </summary>
    public class UserType : IUserType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserType" /> class.
        /// </summary>
        /// <param name="definition">The symbol representing the type definition.</param>
        public UserType(INamedTypeSymbol definition)
        {
            this.Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        /// <summary>
        /// Gets the symbol that represents the type's definition.
        /// </summary>
        public INamedTypeSymbol Definition { get; }

        /// <summary>
        /// Gets a value indicating whether <see cref="Definition" /> is an enum.
        /// </summary>
        public bool IsEnum => this.Definition is IEnum;

        /// <summary>
        /// Gets a value indicating whether <see cref="Definition" /> is a struct.
        /// </summary>
        public bool IsStruct => this.Definition is IStruct;

        /// <inheritdoc/>
        public string Name => this.Definition.Name;

        /// <inheritdoc/>
        public string CSharpOptionalTypeName
        {
            get
            {
                if (string.IsNullOrEmpty(this.Definition.Document.CSharpNamespace))
                {
                    return this.GetOptionalTypeName();
                }

                return $"{this.Definition.Document.CSharpNamespace}.{this.GetOptionalTypeName()}";
            }
        }

        /// <inheritdoc/>
        public string CSharpRequiredTypeName
        {
            get
            {
                if (string.IsNullOrEmpty(this.Definition.Document.CSharpNamespace))
                {
                    return this.Definition.Name;
                }

                return $"{this.Definition.Document.CSharpNamespace}.{this.Definition.Name}";
            }
        }

        /// <inheritdoc/>
        public bool IsBaseType => false;

        /// <inheritdoc/>
        public bool IsList => false;

        /// <inheritdoc/>
        public bool IsCollection => false;

        /// <inheritdoc/>
        public bool IsSet => false;

        /// <inheritdoc/>
        public bool IsMap => false;

        /// <inheritdoc/>
        public bool IsAssignableFrom(IType expressionType)
        {
            return false;
        }

        private string GetOptionalTypeName()
        {
            return this.IsEnum ? this.Definition.Name + "?" : this.Definition.Name;
        }
    }
}