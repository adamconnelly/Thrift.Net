namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Thrift.Net.Compilation;
    using Xunit;

    public class ParseErrorTests : ThriftCompilerTests
    {
        [Fact]
        public void Compile_InputContainsSyntaxErrors_ErrorsReported()
        {
            this.AssertCompilerReturnsErrorMessage(
@"$structe$ User {
    i32 Id
}",
CompilerMessageId.GenericParseError);
        }
    }
}