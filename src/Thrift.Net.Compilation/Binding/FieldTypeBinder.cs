namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Model;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind the type of fields.
    /// </summary>
    public class FieldTypeBinder : Binder<FieldTypeContext, FieldType>, IBinder
    {
        private readonly IBinder parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldTypeBinder" /> class.
        /// </summary>
        /// <param name="parent">The parent binder.</param>
        public FieldTypeBinder(IBinder parent)
            : base(parent)
        {
            this.parent = parent;
        }

        /// <inheritdoc />
        public override FieldType ResolveType(string typeName)
        {
            return this.parent.ResolveType(typeName);
        }

        /// <summary>
        /// Uses the parsed field type to create a <see cref="FieldType" /> object.
        /// </summary>
        /// <param name="context">The parsed type name.</param>
        /// <returns>The field type, or null if the type could not be resolved.</returns>
        protected override FieldType Bind(FieldTypeContext context)
        {
            var typeName = context.IDENTIFIER().Symbol.Text;
            var baseType = FieldType.ResolveBaseType(typeName);
            if (baseType != null)
            {
                return baseType;
            }

            if (typeName.Split('.').Length <= 2)
            {
                var userType = this.parent.ResolveType(typeName);
                if (userType != null)
                {
                    return userType;
                }
            }

            return FieldType.CreateUnresolvedType(typeName);
        }
    }
}