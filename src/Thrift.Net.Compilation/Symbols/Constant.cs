namespace Thrift.Net.Compilation.Symbols
{
    using System;
    using Thrift.Net.Compilation.Binding;
    using Thrift.Net.Compilation.Extensions;
    using Thrift.Net.Compilation.Types;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift constant definition.
    /// </summary>
    public class Constant : NamedSymbol<ConstDefinitionContext, IDocument>, IConstant
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Constant" /> class.
        /// </summary>
        /// <param name="node">The parse tree node this symbol is bound to.</param>
        /// <param name="parent">The document containing the constant.</param>
        /// <param name="name">The name of the constant.</param>
        /// <param name="binderProvider">Used to get binders.</param>
        public Constant(
            ConstDefinitionContext node,
            IDocument parent,
            string name,
            IBinderProvider binderProvider)
            : base(node, parent, name)
        {
            this.binderProvider = binderProvider;
        }

        // TODO: Need to rename this so we don't have Type.Reference.Type going on
        /// <inheritdoc/>
        public IFieldType Type
        {
            get
            {
                return this.binderProvider.GetBinder(this.Node.fieldType())
                    .Bind<IFieldType>(this.Node.fieldType(), this);
            }
        }

        /// <inheritdoc/>
        public IConstantExpression Expression
        {
            get
            {
                if (this.Node.constExpression() != null)
                {
                    return this.binderProvider.GetBinder(this.Node.constExpression())
                        .Bind<IConstantExpression>(this.Node.constExpression(), this);
                }

                return null;
            }
        }

        /// <inheritdoc/>
        public IDocument Document => this.Document;

        /// <inheritdoc/>
        public string CSharpValue
        {
            get
            {
                if (this.Expression == null)
                {
                    throw new InvalidOperationException(
                        "Cannot generate the C# value for this constant because no value could be parsed from the Thrift source");
                }

                if (!this.Type.Reference.Type.IsAssignableFrom(this.Expression.Type))
                {
                    throw new InvalidOperationException(
                        "Cannot generate the C# value for this constant because the value cannot be assigned to the constant's type");
                }

                if (this.Type.Reference.Type == BaseType.Bool && this.Expression.Type.IsIntegerType())
                {
                    return long.Parse(this.Expression.RawValue) > 0 ? "true" : "false";
                }

                return this.Expression.RawValue;
            }
        }

        /// <inheritdoc/>
        public override void Accept(ISymbolVisitor visitor)
        {
            visitor.VisitConstant(this);
            base.Accept(visitor);
        }
    }
}
