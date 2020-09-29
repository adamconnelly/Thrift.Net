namespace Thrift.Net.Compilation.Symbols.Builders
{
    using System.Collections.Generic;
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to build <see cref="Document" /> objects.
    /// </summary>
    public class DocumentBuilder : SymbolBuilder<DocumentContext, Document, ISymbol, DocumentBuilder>
    {
        /// <summary>
        /// Builds the document.
        /// </summary>
        /// <returns>The document.</returns>
        public override Document Build()
        {
            return new Document(this.Node, this.Parent, this.BinderProvider);
        }
    }
}
