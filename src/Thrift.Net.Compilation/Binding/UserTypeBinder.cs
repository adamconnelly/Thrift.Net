namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind <see cref="IUserType" /> objects.
    /// </summary>
    public class UserTypeBinder : Binder<UserTypeContext, IUserType, IField>
    {
        /// <inheritdoc/>
        protected override IUserType Bind(UserTypeContext node, IField parent)
        {
            var type = parent.ResolveType2(node.IDENTIFIER().Symbol.Text);
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