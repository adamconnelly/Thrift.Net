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

        [Fact]
        public void Compile_DuplicateFieldName_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"struct {
    i32 Username
    string $Username$
}",
CompilerMessageId.StructFieldNameAlreadyDefined,
"Username");
        }
    }
}