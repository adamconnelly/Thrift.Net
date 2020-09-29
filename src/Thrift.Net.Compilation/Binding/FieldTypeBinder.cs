namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind the type of fields.
    /// </summary>
    public class FieldTypeBinder : Binder<FieldTypeContext, FieldType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldTypeBinder" /> class.
        /// </summary>
        /// <param name="parent">The parent binder.</param>
        public FieldTypeBinder(IBinder parent)
            : base(parent)
        {
        }

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