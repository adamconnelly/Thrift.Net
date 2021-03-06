namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Thrift.Net.Compilation;
    using Xunit;

    public class EnumWarningTests : ThriftCompilerTests
    {
        [Fact]
        public void Compile_EnumHasNoMembers_ReportsWarning()
        {
            this.AssertCompilerReturnsWarningMessage(
                "enum $UserType$ {}",
                CompilerMessageId.EnumEmpty);
        }

        [Fact]
        public void Compile_EnumWithMissingNameHasNoMembers_ReportsWarning()
        {
            this.AssertCompilerReturnsWarningMessage(
                "$enum$ {}",
                CompilerMessageId.EnumEmpty);
        }

        [Fact]
        public void Compile_EnumMemberDoesNotHaveExplicitValue_ReportsWarning()
        {
            this.AssertCompilerReturnsWarningMessage(
@"enum UserType {
    Administrator = 0
    $User$
}",
CompilerMessageId.EnumMemberHasImplicitValue,
"User",
"1");
        }
    }
}