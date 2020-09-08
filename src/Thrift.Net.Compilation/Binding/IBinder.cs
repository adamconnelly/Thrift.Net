namespace Thrift.Net.Compilation.Binding
{
    using System;
    using Antlr4.Runtime;
    using Thrift.Net.Compilation.Model;

    /// <summary>
    /// An object that can bind a node in the parse tree to its semantic
    /// representation.
    /// </summary>
    public interface IBinder
    {
        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <param name="typeName">The name of the type to resolve.</param>
        /// <returns>The type, or null if the type could not be resolved.</returns>
        FieldType ResolveType(string typeName);

        /// <summary>
        /// Creates a symbol based on the specified node.
        /// </summary>
        /// <param name="node">The node in the tree to get the symbol for.</param>
        /// <typeparam name="TSymbol">The type of the symbol.</typeparam>
        /// <returns>The symbol.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the symbol created from the node cannot be converted to
        /// <typeparamref name="TSymbol" />.
        /// </exception>
        TSymbol Bind<TSymbol>(ParserRuleContext node)
            where TSymbol : class, ISymbol;
    }
}