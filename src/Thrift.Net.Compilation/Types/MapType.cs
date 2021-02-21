namespace Thrift.Net.Compilation.Types
{
    using System;
    using System.Collections.Generic;
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift map.
    /// </summary>
    public class MapType : CollectionType, IMapType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapType" /> class.
        /// </summary>
        /// <param name="keyType">The type of key for the map.</param>
        /// <param name="valueType">The type of value for the map.</param>
        public MapType(IType keyType, IType valueType)
        {
            // TODO: Should we make keyType and valueType not nullable, and use a special UnresolvedType / EmptyType?
            this.KeyType = keyType;
            this.ValueType = valueType;
        }

        /// <inheritdoc/>
        public IType KeyType { get; }

        /// <inheritdoc/>
        public IType ValueType { get; }

        /// <inheritdoc/>
        protected override string GetTypeName()
        {
            return $"System.Collections.Generic.Dictionary<{this.KeyType?.CSharpRequiredTypeName}, {this.ValueType?.CSharpRequiredTypeName}>";
        }

        /// <inheritdoc/>
        protected override string GetThriftTypeName()
        {
            return $"map<{this.KeyType?.Name}, {this.ValueType?.Name}>";
        }
    }
}