namespace Thrift.Net.Compilation.Symbols
{
    using System.Collections.Generic;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents an enum.
    /// </summary>
    public interface IEnum : ISymbol<EnumDefinitionContext, IDocument>, INamedTypeSymbol
    {
        /// <summary>
        /// Gets the enum members.
        /// </summary>
        IReadOnlyCollection<IEnumMember> Members { get; }

        /// <summary>
        /// Checks whether the specified name has already been used by another
        /// enum member.
        /// </summary>
        /// <param name="enumMember">The member we're checking.</param>
        /// <returns>
        /// true if a member with the same name has already been declared, false otherwise.
        /// </returns>
        bool IsEnumMemberAlreadyDeclared(IEnumMember enumMember);

        /// <summary>
        /// Checks whether the specified value has already been used by another
        /// enum member.
        /// </summary>
        /// <param name="enumMember">The member we're checking.</param>
        /// <returns>
        /// true if a member with the same value has already been declared (i.e.
        /// <paramref name="enumMember" /> is a duplicate), false otherwise.
        /// </returns>
        bool IsEnumValueAlreadyDeclared(IEnumMember enumMember);
    }
}