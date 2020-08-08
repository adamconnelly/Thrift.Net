namespace Thrift.Net.Compilation.Model
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
        /// <param name="enums">Any enums found in the document.</param>
        public ThriftDocument(IReadOnlyCollection<EnumDefinition> enums)
        {
            this.Enums = enums;
        }

        /// <summary>
        /// Gets any enums that have been defined.
        /// </summary>
        public IReadOnlyCollection<EnumDefinition> Enums { get; }
    }
}