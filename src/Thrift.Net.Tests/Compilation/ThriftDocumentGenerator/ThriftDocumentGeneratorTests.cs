namespace Thrift.Net.Tests.Compilation.ThriftDocumentGenerator
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using ThriftDocumentGenerator = Thrift.Net.Compilation.ThriftDocumentGenerator;

    public abstract class ThriftDocumentGeneratorTests
    {
        private readonly ThriftDocumentGenerator generator = new ThriftDocumentGenerator();
        private readonly Thrift.Net.Compilation.ThriftCompiler compiler = new Thrift.Net.Compilation.ThriftCompiler();

        protected ThriftDocumentGenerator Generator => this.generator;
        protected Thrift.Net.Compilation.ThriftCompiler Compiler => this.compiler;

        /// <summary>
        /// Parses the specified C# code and returns the root node in the syntax
        /// tree.
        /// </summary>
        /// <param name="input">The C# text to parse.</param>
        /// <returns>The root of the tree.</returns>
        protected static (SyntaxTree syntaxTree, CSharpCompilation compilation, SemanticModel semanticModel) ParseCSharp(
            string input)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(input);
            var compilation = CSharpCompilation
                .Create("Thrift.Net.Tests.Compilation")
                .AddSyntaxTrees(syntaxTree);
            var semanticModel = compilation.GetSemanticModel(syntaxTree);

            return (syntaxTree, compilation, semanticModel);
        }
    }
}
