namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift exception.
    /// </summary>
    public interface IException : ISymbol<ExceptionDefinitionContext, IDocument>, INamedTypeSymbol, IFieldContainer
    {
    }
}