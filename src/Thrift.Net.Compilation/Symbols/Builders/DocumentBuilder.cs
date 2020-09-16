namespace Thrift.Net.Compilation.Symbols.Builders
{
    using System.Collections.Generic;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to build <see cref="Document" /> objects.
    /// </summary>
    public class DocumentBuilder
    {
        private readonly List<Namespace> namespaces = new List<Namespace>();
        private readonly List<Enum> enums = new List<Enum>();
        private readonly List<Struct> structs = new List<Struct>();

        /// <summary>
        /// Gets the node the document was created from.
        /// </summary>
        public DocumentContext Node { get; private set; }

        /// <summary>
        /// Gets any namespaces that have been defined.
        /// </summary>
        public IReadOnlyCollection<Namespace> Namespaces => this.namespaces;

        /// <summary>
        /// Gets any enums that have been defined.
        /// </summary>
        public IReadOnlyCollection<Enum> Enums => this.enums;

        /// <summary>
        /// Gets any structs that have been defined.
        /// </summary>
        public IReadOnlyCollection<Struct> Structs => this.structs;

        /// <summary>
        /// Sets the node the document was created from.
        /// </summary>
        /// <param name="node">The node the document was created from.</param>
        /// <returns>The builder.</returns>
        public DocumentBuilder SetNode(DocumentContext node)
        {
            this.Node = node;

            return this;
        }

        /// <summary>
        /// Adds the enum to the document.
        /// </summary>
        /// <param name="enum">The enum.</param>
        /// <returns>The builder.</returns>
        public DocumentBuilder AddEnum(Enum @enum)
        {
            this.enums.Add(@enum);

            return this;
        }

        /// <summary>
        /// Adds the enum to the document.
        /// </summary>
        /// <param name="configure">The action to configure the enum.</param>
        /// <returns>The builder.</returns>
        public DocumentBuilder AddEnum(System.Action<EnumBuilder> configure)
        {
            var enumBuilder = new EnumBuilder();
            configure(enumBuilder);

            this.AddEnum(enumBuilder.Build());

            return this;
        }

        /// <summary>
        /// Adds the enums to the document.
        /// </summary>
        /// <param name="enums">The enums to add.</param>
        /// <returns>The builder.</returns>
        public DocumentBuilder AddEnums(IEnumerable<Enum> enums)
        {
            foreach (var @enum in enums)
            {
                this.AddEnum(@enum);
            }

            return this;
        }

        /// <summary>
        /// Adds the struct to the document.
        /// </summary>
        /// <param name="struct">The struct to add.</param>
        /// <returns>The builder.</returns>
        public DocumentBuilder AddStruct(Struct @struct)
        {
            this.structs.Add(@struct);

            return this;
        }

        /// <summary>
        /// Adds the struct to the document.
        /// </summary>
        /// <param name="configure">The action to configure the struct.</param>
        /// <returns>The builder.</returns>
        public DocumentBuilder AddStruct(System.Action<StructBuilder> configure)
        {
            var structBuilder = new StructBuilder();
            configure(structBuilder);

            this.AddStruct(structBuilder.Build());

            return this;
        }

        /// <summary>
        /// Adds the structs to the document.
        /// </summary>
        /// <param name="structs">The structs to add.</param>
        /// <returns>The builder.</returns>
        public DocumentBuilder AddStructs(IEnumerable<Struct> structs)
        {
            foreach (var @struct in structs)
            {
                this.AddStruct(@struct);
            }

            return this;
        }

        /// <summary>
        /// Adds the namespace to the document.
        /// </summary>
        /// <param name="namespace">The namespace to add.</param>
        /// <returns>The builder.</returns>
        public DocumentBuilder AddNamespace(Namespace @namespace)
        {
            this.namespaces.Add(@namespace);

            return this;
        }

        /// <summary>
        /// Adds the namespace to the document.
        /// </summary>
        /// <param name="configure">Used to configure the namespace.</param>
        /// <returns>The builder.</returns>
        public DocumentBuilder AddNamespace(System.Action<NamespaceBuilder> configure)
        {
            var namespaceBuilder = new NamespaceBuilder();
            configure(namespaceBuilder);

            return this.AddNamespace(namespaceBuilder.Build());
        }

        /// <summary>
        /// Adds the namespaces to the document.
        /// </summary>
        /// <param name="namespaces">The namespaces to add.</param>
        /// <returns>The builder.</returns>
        public DocumentBuilder AddNamespaces(IEnumerable<Namespace> namespaces)
        {
            foreach (var @namespace in namespaces)
            {
                this.AddNamespace(@namespace);
            }

            return this;
        }

        /// <summary>
        /// Builds the document.
        /// </summary>
        /// <returns>The document.</returns>
        public Document Build()
        {
            return new Document(
                this.Node,
                this.Namespaces,
                this.Enums,
                this.Structs);
        }
    }
}
