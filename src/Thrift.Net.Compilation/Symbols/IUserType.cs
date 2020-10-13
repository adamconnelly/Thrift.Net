namespace Thrift.Net.Compilation.Symbols
{
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a user-defined type like a struct or enum.
    /// </summary>
    public interface IUserType : ISymbol<UserTypeContext, ISymbol>, IFieldType
    {
    }
}