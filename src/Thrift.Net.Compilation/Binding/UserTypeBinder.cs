namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind <see cref="IUserType" /> objects.
    /// </summary>
    public class UserTypeBinder : Binder<UserTypeContext, IUserType, ISymbol>
    {
        /// <inheritdoc/>
        protected override IUserType Bind(UserTypeContext node, ISymbol parent)
        {
            var type = parent.ResolveType(node.IDENTIFIER().Symbol.Text);
            if (type == null)
            {
                type = new UnresolvedType(node, parent, node.IDENTIFIER().Symbol.Text);
            }

            return new UserTypeBuilder()
                .SetNode(node)
                .SetParent(parent)
                .SetDefinition(type)
                .Build();
        }
    }
}