namespace Thrift.Net.Compilation.Binding
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Antlr;
    using Thrift.Net.Compilation.Model;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// The BinderProvider is used to get <see cref="IBinder" /> objects that
    /// can be used to create <see cref="ISymbol" /> objects from the parse tree.
    /// </summary>
    public class BinderProvider : IBinderProvider
    {
        private readonly ParseTreeProperty<IBinder> binderMap = new ParseTreeProperty<IBinder>();
        private readonly BinderVisitor binderVisitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinderProvider" /> class.
        /// </summary>
        public BinderProvider()
        {
            this.binderVisitor = new BinderVisitor(this, this.binderMap);
        }

        /// <summary>
        /// Adds the nodes in the document to the set that binders can be
        /// provided for.
        /// </summary>
        /// <param name="documentContext">The document.</param>
        public void AddDocument(DocumentContext documentContext)
        {
            documentContext.Accept(this.binderVisitor);
        }

        /// <summary>
        /// Gets the binder for the specified node.
        /// </summary>
        /// <param name="node">The node to get the binder for.</param>
        /// <returns>
        /// The binder, or null if no binder exists for the specified node.
        /// </returns>
        public IBinder GetBinder(IParseTree node)
        {
            return this.binderMap.Get(node);
        }

        private class BinderVisitor : ThriftBaseVisitor<IBinder>
        {
            private readonly ParseTreeProperty<IBinder> binderMap;
            private readonly IBinderProvider binderProvider;

            public BinderVisitor(
                IBinderProvider binderProvider, ParseTreeProperty<IBinder> binderMap)
            {
                this.binderProvider = binderProvider;
                this.binderMap = binderMap;
            }

            public override IBinder VisitField([NotNull] ThriftParser.FieldContext context)
            {
                var containerBinder = this.binderMap.Get(context.Parent) as IFieldContainerBinder;
                var binder = new FieldBinder(containerBinder, this.binderProvider);
                this.binderMap.Put(context, binder);

                base.VisitField(context);

                return binder;
            }

            public override IBinder VisitFieldType([NotNull] FieldTypeContext context)
            {
                base.VisitFieldType(context);

                var binder = new FieldTypeBinder(this.binderMap.Get(context.Parent));
                this.binderMap.Put(context, binder);

                return binder;
            }

            public override IBinder VisitStructDefinition([NotNull] StructDefinitionContext context)
            {
                // TODO: Pass in document binder
                var structBinder = new StructBinder(null, this.binderProvider);
                this.binderMap.Put(context, structBinder);

                base.VisitStructDefinition(context);

                return structBinder;
            }
        }
    }
}