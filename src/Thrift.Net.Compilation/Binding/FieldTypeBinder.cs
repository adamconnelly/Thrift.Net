namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind the type of fields.
    /// </summary>
    public class FieldTypeBinder : Binder<FieldTypeContext, FieldType>
    {
        /// <inheritdoc />
        protected override FieldType Bind(FieldTypeContext node, ISymbol parent)
        {
            var typeName = node.IDENTIFIER().Symbol.Text;
            var baseType = FieldType.ResolveBaseType(typeName);
            if (baseType != null)
            {
                return baseType;
            }

            if (typeName.Split('.').Length <= 2)
            {
                var userType = parent.ResolveType(typeName);
                if (userType != null)
                {
                    return userType;
                }
            }

            return FieldType.CreateUnresolvedType(node, typeName);
        }
    }
}