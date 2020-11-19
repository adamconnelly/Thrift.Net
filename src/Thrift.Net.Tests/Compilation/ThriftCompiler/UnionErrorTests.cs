namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Xunit;

    public class UnionErrorTests : ThriftCompilerTests
    {
        [Fact]
        public void UnionContainsRequiredFields_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"union Request {
    1: $required$ string Query
}",
Net.Compilation.CompilerMessageId.UnionCannotContainRequiredFields,
"Query");
        }

        [Fact]
        public void UnionNameMissing_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"$union$ {
    1: string Query
}",
Net.Compilation.CompilerMessageId.UnionMustHaveAName);
        }

        [Fact]
        public void UnionNameAlreadyUsed_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"struct Request {}
union $Request$ {
    1: string Query
}",
Net.Compilation.CompilerMessageId.NameAlreadyDeclared,
"Request");
        }
    }
}