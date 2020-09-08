namespace Thrift.Net.Compilation.Binding
{
    using Antlr4.Runtime;

    /// <summary>
    /// An object that can provide the correct <see cref="IBinder" /> for nodes
    /// in the parse tree.
    /// </summary>
    public interface IBinderProvider
    {
        /// <summary>
        /// Gets the binder for the specified node.
        /// </summary>
        /// <param name="node">The node in the tree to get the binder for.</param>
        /// <returns>The binder for the specified node.</returns>
        IBinder GetBinder(ParserRuleContext node);
    }
}