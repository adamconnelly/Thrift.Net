namespace Thrift.Net.Tests.Compilation.ThriftDocumentGenerator
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Thrift.Net.Compilation;
    using ThriftDocumentGenerator = Thrift.Net.Compilation.ThriftDocumentGenerator;

    public abstract class ThriftDocumentGeneratorTests
    {
        private readonly ThriftDocumentGenerator generator = new ThriftDocumentGenerator();
        private readonly ThriftCompiler compiler = new ThriftCompiler();
        private readonly ThriftFile thriftFile = new ThriftFile("Test.thrift", "Test.thrift", "Test.thrift", "output");

        protected ThriftDocumentGenerator Generator => this.generator;
        protected ThriftCompiler Compiler => this.compiler;
        protected ThriftFile ThriftFile => this.thriftFile;

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
