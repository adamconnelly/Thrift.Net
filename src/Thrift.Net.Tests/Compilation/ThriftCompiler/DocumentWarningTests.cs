namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Thrift.Net.Compilation;
    using Xunit;

    public class DocumentWarningTests : ThriftCompilerTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("namespace * Thrift.Net.Examples")]
        [InlineData("include 'UserType.thrift'")]
        [InlineData("cpp_include '<unordered_set>'")]
        public void DocumentEmpty_ReportsWarning(string input)
        {
            this.AssertCompilerReturnsWarningMessage(
                input,
                CompilerMessageId.DocumentEmpty);
        }

        [Theory]
        [InlineData("enum UserType {}")]
        [InlineData("struct User {}")]
        public void DocumentContainsDefinition_DoesNotReportWarning(string input)
        {
            this.AssertCompilerDoesNotReturnMessage(
                input, CompilerMessageId.DocumentEmpty);
        }
    }
}