namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Thrift.Net.Compilation;
    using Xunit;

    public class EnumWarningTests : ThriftCompilerTests
    {
        [Fact]
        public void Compile_EnumHasNoMembers_ReportsWarning()
        {
            this.AssertCompilerReturnsWarning(
                "enum $UserType$ {}",
                CompilerMessageId.EnumEmpty);
        }

        [Fact]
        public void Compile_EnumWithMissingNameHasNoMembers_ReportsWarning()
        {
            this.AssertCompilerReturnsWarning(
                "$enum$ {}",
                CompilerMessageId.EnumEmpty);
        }
    }
}