namespace Thrift.Net.Compilation.Types
{
    using System;

    /// <summary>
    /// Represents a reference to a type.
    /// </summary>
    public class TypeReference : ITypeReference
    {
        /// <summary>
        /// Represents an unresolved type.
        /// </summary>
        public static readonly TypeReference Unresolved = new TypeReference();

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeReference" /> class.
        /// </summary>
        /// <param name="type">The type that is being referenced.</param>
        public TypeReference(IType type)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.IsResolved = true;
        }

        private TypeReference()
        {
        }

        /// <summary>
        /// Gets a value indicating whether the type could be resolved.
        /// </summary>
        public bool IsResolved { get; }

        /// <summary>
        /// Gets the type that is being referenced.
        /// </summary>
        public IType Type { get; }
    }
}