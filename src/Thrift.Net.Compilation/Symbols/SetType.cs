namespace Thrift.Net.Compilation.Symbols
{
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Compilation.Binding;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Represents a Thrift set.
    /// </summary>
    public class SetType : ListOrSetType<SetTypeContext>, ISetType
    {
        /// <summary>
        /// The Thrift type name for a set.
        /// </summary>
        public const string ThriftTypeName = "set";

        /// <summary>
        /// The name of the C# type to use to generate a set.
        /// </summary>
        public const string CSharpTypeName = "System.Collections.Generic.HashSet";

        /// <summary>
        /// Initializes a new instance of the <see cref="SetType" /> class.
        /// </summary>
        /// <param name="node">The node this symbol is bound to.</param>
        /// <param name="parent">This symbol's parent.</param>
        /// <param name="binderProvider">Used to bind child symbols.</param>
        public SetType(SetTypeContext node, ISymbol parent, IBinderProvider binderProvider)
            : base(node, parent, binderProvider, CSharpTypeName)
        {
        }

        /// <inheritdoc/>
        public override void Accept(ISymbolVisitor visitor)
        {
            visitor.VisitSetType(this);
            base.Accept(visitor);
        }

        /// <inheritdoc/>
        protected override IParseTree GetElementNode()
        {
            return this.Node.fieldType();
        }
    }
}