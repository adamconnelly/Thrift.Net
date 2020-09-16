namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using System.Linq;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Describes a Thrift IDL file.
    /// </summary>
    public class Document : Symbol<DocumentContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Document" /> class.
        /// </summary>
        /// <param name="node">The document node.</param>
        /// <param name="namespaces">The namespaces defined in the document.</param>
        /// <param name="enums">Any enums found in the document.</param>
        /// <param name="structs">Any structs found in the document.</param>
        public Document(
            DocumentContext node,
            IReadOnlyCollection<Namespace> @namespaces,
            IReadOnlyCollection<Enum> enums,
            IReadOnlyCollection<Struct> structs)
            : base(node)
        {
            this.Namespaces = @namespaces;
            this.Enums = enums;
            this.Structs = structs;
        }

        /// <summary>
        /// Gets the C# namespace of the document.
        /// </summary>
        public IReadOnlyCollection<Namespace> Namespaces { get; }

        /// <summary>
        /// Gets any enums that have been defined.
        /// </summary>
        public IReadOnlyCollection<Enum> Enums { get; }

        /// <summary>
        /// Gets any structs that have been defined.
        /// </summary>
        public IReadOnlyCollection<Struct> Structs { get; }

        /// <summary>
        /// Gets the C# namespace that should be used for generating this document.
        /// </summary>
        /// <remarks>
        /// If multiple valid namespace scopes are provided, the compiler will
        /// use the most specific namespace and fallback to the namespace with the
        /// `*` scope if no valid C# scopes are specified. If multiple C# scopes
        /// are specified (e.g. `csharp` and `netstd`), the last one defined in
        /// the document will be used.
        /// </remarks>
        public string CSharpNamespace
        {
            get
            {
                var @namespace = this.Namespaces
                    .LastOrDefault(n => n.HasCSharpScope);
                if (@namespace != null)
                {
                    return @namespace.NamespaceName;
                }

                return this.Namespaces
                    .FirstOrDefault(n => n.AppliesToAllTargets)?.NamespaceName;
            }
        }
    }
}
