namespace Thrift.Net.Compilation.Binding
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Thrift.Net.Antlr;
    using Thrift.Net.Compilation.Extensions;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to Bind <see cref="EnumMember" /> Symbols from
    /// <see cref="EnumMemberContext" /> nodes.
    /// </summary>
    public class EnumMemberBinder : Binder<EnumMemberContext, EnumMember, IEnum>
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumMemberBinder" /> class.
        /// </summary>
        /// <param name="binderProvider">Used to get binders for fields.</param>
        public EnumMemberBinder(IBinderProvider binderProvider)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc />
        protected override EnumMember Bind(EnumMemberContext node, IEnum parent)
        {
            var builder = new EnumMemberBuilder()
                .SetNode(node)
                .SetParent(parent)
                .SetBinderProvider(this.binderProvider)
                .SetName(node.IDENTIFIER()?.Symbol.Text)
                .SetRawValue(node.enumValue?.Text)
                .SetIsValueImplicit(node.EQUALS_OPERATOR() == null && node.enumValue == null);

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
                var (value, reason) = this.GetEnumValue(node);
                builder.SetValue(value);
                builder.SetInvalidValueReason(reason);
            }
            else if (node.EQUALS_OPERATOR() == null)
            {
                builder.SetValue(this.CalculateAutomaticValue(node));
            }
        }

        private (int?, InvalidEnumValueReason) GetEnumValue(EnumMemberContext node)
        {
            if (node.enumValue.Type == ThriftParser.HEX_CONSTANT)
            {
                var hexText = node.enumValue.Text.AsSpan()[2..];

                if (int.TryParse(hexText, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var hexValue))
                {
                    return (hexValue, InvalidEnumValueReason.None);
                }

                return (null, InvalidEnumValueReason.InvalidHexValue);
            }

            if (int.TryParse(node.enumValue.Text, out var value))
            {
                if (value >= 0)
                {
                    return (value, InvalidEnumValueReason.None);
                }

                return (null, InvalidEnumValueReason.Negative);
            }

            return (null, InvalidEnumValueReason.NotAnInteger);
        }

        /// <summary>
        /// Calculates the automatic enum value for members that don't have an
        /// explicit value specified.
        /// </summary>
        /// <param name="node">The member node.</param>
        /// <remarks>
        /// The first member in an enum is given a value of 0, and the other members
        /// are assigned N + 1, where N is the value of the previous enum member.
        /// </remarks>
        private int CalculateAutomaticValue(EnumMemberContext node)
        {
            var parent = node.Parent as EnumDefinitionContext;

            // If there's only a single member, don't bother doing any more work
            if (parent == null || parent.enumMember().Length == 1)
            {
                return 0;
            }

            var value = parent.enumMember()
                .TakeUntil(member => member == node)
                .Aggregate(-1, (currentValue, currentNode) =>
                {
                    if (currentNode.enumValue != null)
                    {
                        var (value, _) = this.GetEnumValue(currentNode);
                        if (value != null)
                        {
                            return value.Value;
                        }

                        return currentValue;
                    }

                    return currentValue + 1;
                });

            return value;
        }
    }
}