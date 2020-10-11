namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind the type of fields.
    /// </summary>
    public class FieldTypeBinder : Binder<FieldTypeContext, FieldType, IField>
    {
        /// <inheritdoc />
        protected override FieldType Bind(FieldTypeContext node, IField parent)
        {
            var typeName = node.GetText();
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

            return FieldType.CreateUnresolvedType(typeName);
        }
    }
}