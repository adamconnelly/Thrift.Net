namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Thrift.Net.Compilation;
    using Xunit;

    public class StructErrorTests : ThriftCompilerTests
    {
        [Fact]
        public void Compile_EnumNameMissing_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
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

        [Fact]
        public void Compile_FieldIdNotAnInt_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"struct User {
    $abc$: string Username
}",
CompilerMessageId.StructFieldIdMustBeAPositiveInteger,
"abc");
        }

        [Fact]
        public void Compile_FieldIdNegative_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"struct User {
    $-5$: string Username
}",
CompilerMessageId.StructFieldIdMustBeAPositiveInteger,
"-5");
        }
    }
}