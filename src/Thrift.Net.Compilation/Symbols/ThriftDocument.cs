namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;

    /// <summary>
    /// Describes a Thrift IDL file.
    /// </summary>
    public class ThriftDocument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThriftDocument" /> class.
        /// </summary>
        /// <param name="namespace">The C# namespace of the document.</param>
        /// <param name="enums">Any enums found in the document.</param>
        /// <param name="structs">Any structs found in the document.</param>
        public ThriftDocument(
            string @namespace,
            IReadOnlyCollection<EnumDefinition> enums,
            IReadOnlyCollection<StructDefinition> structs)
        {
            this.Namespace = @namespace;
            this.Enums = enums;
            this.Structs = structs;
        }

        /// <summary>
        /// Gets the C# namespace of the document.
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// Gets any enums that have been defined.
        /// </summary>
        public IReadOnlyCollection<EnumDefinition> Enums { get; }

        /// <summary>
        /// Gets any structs that have been defined.
        /// </summary>
        public IReadOnlyCollection<StructDefinition> Structs { get; }
    }
}