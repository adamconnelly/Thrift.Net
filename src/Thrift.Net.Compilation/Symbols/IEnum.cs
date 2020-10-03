namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents an enum.
    /// </summary>
    public interface IEnum : ISymbol<EnumDefinitionContext, IDocument>, INamedSymbol
    {
        /// <summary>
        /// Gets the enum members.
        /// </summary>
        IReadOnlyCollection<IEnumMember> Members { get; }

        /// <summary>
        /// Checks whether the specified name has already been used by another
        /// enum member.
        /// </summary>
        /// <param name="memberName">The name to check for.</param>
        /// <param name="node">The node being defined.</param>
        /// <returns>
        /// true if a member with the same name has already been declared, false otherwise.
        /// </returns>
        bool IsEnumMemberAlreadyDeclared(string memberName, EnumMemberContext node);
    }
}