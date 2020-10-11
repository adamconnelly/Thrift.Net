namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Binds a <see cref="FieldTypeContext" /> node to a <see cref="BaseType" />
    /// object.
    /// </summary>
    public class BaseTypeBinder : Binder<BaseTypeContext, BaseType, IField>
    {
        /// <inheritdoc />
        protected override BaseType Bind(BaseTypeContext node, IField parent)
        {
            return BaseType.Resolve(node, parent);
        }
    }
}