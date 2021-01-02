namespace Thrift.Net.Compilation.Binding
{
    using System;
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Compilation.Symbols;
    using static Thrift.Net.Antlr.ThriftParser;

    /// <summary>
    /// The BinderProvider is used to get <see cref="IBinder" /> objects that
    /// can be used to create <see cref="ISymbol" /> objects from the parse tree.
    /// </summary>
    public class BinderProvider : IBinderProvider
    {
        private static readonly DocumentBinder DocumentBinder;
        private static readonly NamespaceBinder NamespaceBinder;
        private static readonly StructBinder StructBinder;
        private static readonly UnionBinder UnionBinder;
        private static readonly ExceptionBinder ExceptionBinder;
        private static readonly EnumBinder EnumBinder;
        private static readonly EnumMemberBinder EnumMemberBinder;
        private static readonly FieldBinder FieldBinder;
        private static readonly FieldTypeBinder FieldTypeBinder;
        private static readonly BaseTypeBinder BaseTypeBinder;
        private static readonly UserTypeBinder UserTypeBinder;
        private static readonly ListTypeBinder ListTypeBinder;
        private static readonly SetTypeBinder SetTypeBinder;
        private static readonly MapTypeBinder MapTypeBinder;
        private static readonly ConstantBinder ConstantBinder;
        private static readonly ConstantValueBinder ConstantValueBinder;
        private static readonly BinderProvider ProviderInstance;

        static BinderProvider()
        {
            ProviderInstance = new BinderProvider();
            DocumentBinder = new DocumentBinder(ProviderInstance);
            NamespaceBinder = new NamespaceBinder(ProviderInstance);
            StructBinder = new StructBinder(ProviderInstance);
            UnionBinder = new UnionBinder(ProviderInstance);
            ExceptionBinder = new ExceptionBinder(ProviderInstance);
            EnumBinder = new EnumBinder(ProviderInstance);
            EnumMemberBinder = new EnumMemberBinder(ProviderInstance);
            FieldBinder = new FieldBinder(ProviderInstance);
            FieldTypeBinder = new FieldTypeBinder(ProviderInstance);
            BaseTypeBinder = new BaseTypeBinder();
            UserTypeBinder = new UserTypeBinder();
            ListTypeBinder = new ListTypeBinder(ProviderInstance);
            MapTypeBinder = new MapTypeBinder(ProviderInstance);
            SetTypeBinder = new SetTypeBinder(ProviderInstance);
            ConstantBinder = new ConstantBinder(ProviderInstance);
            ConstantValueBinder = new ConstantValueBinder();
        }

        private BinderProvider()
        {
        }

        /// <summary>
        /// Gets the instance of the Binder provider.
        /// </summary>
        public static IBinderProvider Instance
        {
            get { return ProviderInstance; }
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
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (node is DocumentContext)
            {
                return DocumentBinder;
            }
            else if (node is NamespaceStatementContext)
            {
                return NamespaceBinder;
            }
            else if (node is StructDefinitionContext)
            {
                return StructBinder;
            }
            else if (node is UnionDefinitionContext)
            {
                return UnionBinder;
            }
            else if (node is EnumDefinitionContext)
            {
                return EnumBinder;
            }
            else if (node is EnumMemberContext)
            {
                return EnumMemberBinder;
            }
            else if (node is FieldContext)
            {
                return FieldBinder;
            }
            else if (node is FieldTypeContext)
            {
                return FieldTypeBinder;
            }
            else if (node is BaseTypeContext)
            {
                return BaseTypeBinder;
            }
            else if (node is UserTypeContext)
            {
                return UserTypeBinder;
            }
            else if (node is ListTypeContext)
            {
                return ListTypeBinder;
            }
            else if (node is SetTypeContext)
            {
                return SetTypeBinder;
            }
            else if (node is MapTypeContext)
            {
                return MapTypeBinder;
            }
            else if (node is ExceptionDefinitionContext)
            {
                return ExceptionBinder;
            }
            else if (node is ConstDefinitionContext)
            {
                return ConstantBinder;
            }
            else if (node is ConstExpressionContext)
            {
                return ConstantValueBinder;
            }

            throw new ArgumentException(
                $"No binder could be found to bind the {node.GetType().Name} node type",
                nameof(node));
        }
    }
}