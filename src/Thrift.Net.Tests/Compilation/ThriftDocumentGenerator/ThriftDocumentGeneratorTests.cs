namespace Thrift.Net.Tests.Compilation.ThriftDocumentGenerator
{
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public abstract class ThriftDocumentGeneratorTests
    {
        /// <summary>
        /// Parses the specified C# code and returns the root node in the syntax
        /// tree.
        /// </summary>
        /// <param name="input">The C# text to parse.</param>
        /// <returns>The root of the tree.</returns>
        protected static CompilationUnitSyntax ParseCSharp(string input)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(input);

            return syntaxTree.GetCompilationUnitRoot();
        }
    }
}
