namespace Thrift.Net.Compilation.Binding
{
    using Thrift.Net.Compilation.Symbols;
    using Thrift.Net.Compilation.Symbols.Builders;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// Used to bind a <see cref="Struct" /> from the parse tree.
    /// </summary>
    public class ExceptionBinder : Binder<ExceptionDefinitionContext, Exception, IDocument>
    {
        private readonly IBinderProvider binderProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBinder" /> class.
        /// </summary>
        /// <param name="binderProvider">
        /// Used to get the correct binder for a particular node.
        /// </param>
        public ExceptionBinder(IBinderProvider binderProvider)
        {
            this.binderProvider = binderProvider;
        }

        /// <inheritdoc />
        protected override Exception Bind(ExceptionDefinitionContext node, IDocument parent)
        {
            var builder = new ExceptionBuilder()
                .SetNode(node)
                .SetParent(parent)
                .SetBinderProvider(this.binderProvider)
                .SetName(node.name?.Text);

            return builder.Build();
        }
    }
}