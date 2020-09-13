namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to Bind <see cref="EnumMember" /> Symbols from
    /// <see cref="EnumMemberContext" /> nodes.
    /// </summary>
    public class EnumMemberBinder : Binder<EnumMemberContext, EnumMember, IEnumBinder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumMemberBinder" /> class.
        /// </summary>
        /// <param name="parent">The parent binder.</param>
        public EnumMemberBinder(IEnumBinder parent)
            : base(parent)
        {
        }

        /// <inheritdoc />
        protected override EnumMember Bind(EnumMemberContext node)
        {
            var builder = new EnumMemberBuilder()
                .SetNode(node)
                .SetName(node.IDENTIFIER()?.Symbol.Text)
                .SetRawValue(node.enumValue?.Text);

            this.SetEnumValue(node, builder);

            if (node.EQUALS_OPERATOR() != null && node.enumValue == null)
            {
                builder.SetInvalidValueReason(InvalidEnumValueReason.Missing);
            }

            return builder.Build();
        }

        private void SetEnumValue(EnumMemberContext node, EnumMemberBuilder builder)
        {
            if (node.enumValue != null)
            {
                if (int.TryParse(node.enumValue.Text, out var value))
                {
                    if (value >= 0)
                    {
                        builder.SetValue(value);
                    }
                    else
                    {
                        builder.SetInvalidValueReason(InvalidEnumValueReason.Negative);
                    }
                }
                else
                {
                    builder.SetInvalidValueReason(InvalidEnumValueReason.NotAnInteger);
                }
            }
            else if (node.EQUALS_OPERATOR() == null)
            {
                builder.SetValue(this.Parent.GetEnumValue(node));
            }
        }
    }
}