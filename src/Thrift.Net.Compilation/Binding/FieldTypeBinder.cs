namespace Thrift.Net.Compilation.Binding
{
    using System;
    using Thrift.Net.Compilation.Symbols;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind the type of fields.
    /// </summary>
    public class FieldTypeBinder : Binder<FieldTypeContext, IFieldType, ISymbol>
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldTypeBinder" /> class.
        /// </summary>
        /// <param name="binderProvider">Used to get binders for nodes.</param>
        public FieldTypeBinder(IBinderProvider binderProvider)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc />
        protected override IFieldType Bind(FieldTypeContext node, ISymbol parent)
        {
            // TODO: Delete the binders for the individual types and update FieldType
            // to resolve the type
            if (node.baseType() != null)
            {
                return this.binderProvider
                    .GetBinder(node.baseType())
                    .Bind<IFieldType>(node.baseType(), parent);
            }

            if (node.userType() != null)
            {
                return this.binderProvider
                    .GetBinder(node.userType())
                    .Bind<IFieldType>(node.userType(), parent);
            }

            if (node.collectionType()?.listType() != null)
            {
                return this.binderProvider
                    .GetBinder(node.collectionType().listType())
                    .Bind<IFieldType>(node.collectionType().listType(), parent);
            }

            if (node.collectionType()?.setType() != null)
            {
                return this.binderProvider
                    .GetBinder(node.collectionType().setType())
                    .Bind<IFieldType>(node.collectionType().setType(), parent);
            }

            if (node.collectionType()?.mapType() != null)
            {
                return this.binderProvider
                    .GetBinder(node.collectionType().mapType())
                    .Bind<IFieldType>(node.collectionType().mapType(), parent);
            }

            throw new ArgumentException("The node could not be bound to a field type symbol.", nameof(node));
        }
    }
}