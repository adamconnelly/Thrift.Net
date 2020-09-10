namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Thrift.Net.Compilation;
    using Xunit;

    public class StructErrorTests : ThriftCompilerTests
    {
        [Fact]
        public void Compile_EnumNameMissing_ReportsError()
        {
            this.AssertCompilerReturnsErrorId(
                "$struct$ {}",
                CompilerMessageId.StructMustHaveAName);
        }
    }
}