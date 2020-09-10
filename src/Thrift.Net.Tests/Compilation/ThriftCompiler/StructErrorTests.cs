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
@"struct User {
    i32 Username
    string $Username$
}",
CompilerMessageId.StructFieldNameAlreadyDefined,
"Username");
        }

        [Fact]
        public void Compile_DuplicateFieldId_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"struct User {
    1: i32 Id
    $1$: string Username
}",
CompilerMessageId.StructFieldIdAlreadyDefined,
"1");
        }

        // TODO: Error if field Id isn't an int
    }
}