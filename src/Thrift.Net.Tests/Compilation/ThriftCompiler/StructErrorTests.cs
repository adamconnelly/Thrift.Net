namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Thrift.Net.Compilation;
    using Xunit;

    public class StructErrorTests : ThriftCompilerTests
    {
        [Fact]
        public void Compile_StructNameMissing_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
                "$struct$ {}",
                CompilerMessageId.StructMustHaveAName);
        }

        [Fact]
        public void Compile_StructNameAlreadyUsed_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"enum User {}
struct $User$ {}",
CompilerMessageId.NameAlreadyDeclared,
"User");
        }
    }
}