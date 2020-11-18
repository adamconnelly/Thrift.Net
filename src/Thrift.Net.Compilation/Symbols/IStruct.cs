namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift struct.
    /// </summary>
    public interface IStruct : ISymbol<StructDefinitionContext, IDocument>, INamedTypeSymbol, IFieldContainer
    {
    }
}