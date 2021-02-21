namespace Thrift.Net.Compilation.Binding
{
    using System;
    using System.Globalization;
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using Thrift.Net.Compilation.Types;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind <see cref="IConstantExpression" /> symbols from <see cref="ConstExpressionContext" />
    /// nodes.
    /// </summary>
    public class ConstantExpressionBinder : Binder<ConstExpressionContext, IConstantExpression, IConstant>
    {
        /// <inheritdoc/>
        protected override IConstantExpression Bind(ConstExpressionContext node, IConstant parent)
        {
            var type = GetExpressionType(node);

            var builder = new ConstantExpressionBuilder()
                .SetNode(node)
                .SetParent(parent)
                .SetRawValue(node.value?.Text)
                .SetType(type);

            return builder.Build();
        }

        /// <summary>
        /// Gets the type of the constant expression to allow us to check that
        /// the expression is assignable to the constant.
        /// </summary>
        /// <remarks>
        /// When calculating the type for numeric types, we attempt to use the
        /// smallest possible type that the expression could be represented using.
        /// So for integers we'll use the following order: i8, i16, 132, 164. This
        /// means that we can automatically figure out whether or not an expression
        /// is assignable to the constant type definition using the principle that
        /// a smaller type can be assigned to a larger type, but not the other way
        /// round.
        /// </remarks>
        private static BaseType GetExpressionType(ConstExpressionContext node)
        {
            if (node.INT_CONSTANT() != null && long.TryParse(node.value?.Text, out var value))
            {
                return GetTypeFromNumericValue(value);
            }

            if (node.HEX_CONSTANT() != null)
            {
                var hexText = node.HEX_CONSTANT().Symbol.Text.AsSpan()[2..];

                if (long.TryParse(hexText, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var hexValue))
                {
                    return GetTypeFromNumericValue(hexValue);
                }
            }

            if (node.DOUBLE_CONSTANT() != null)
            {
                return BaseType.Double;
            }

            if (node.BOOL_CONSTANT() != null)
            {
                return BaseType.Bool;
            }

            return BaseType.String;
        }

        private static BaseType GetTypeFromNumericValue(long value)
        {
            if (value < int.MinValue || value > int.MaxValue)
            {
                return BaseType.I64;
            }
            else if (value < short.MinValue || value > short.MaxValue)
            {
                return BaseType.I32;
            }
            else if (value < sbyte.MinValue || value > sbyte.MaxValue)
            {
                return BaseType.I16;
            }

            return BaseType.I8;
        }
    }
}