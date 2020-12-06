namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Xunit;

    public class ExceptionErrorTests : ThriftCompilerTests
    {
        [Fact]
        public void ExceptionNameMissing_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"$exception$ {
    1: string Query
}",
Thrift.Net.Compilation.CompilerMessageId.ExceptionMustHaveAName);
        }

        [Fact]
        public void ExceptionNameAlreadyUsed_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"struct Request {}
exception $Request$ {
    1: string Query
}",
Thrift.Net.Compilation.CompilerMessageId.NameAlreadyDeclared,
"Request");
        }
    }
}