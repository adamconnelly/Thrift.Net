namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Thrift.Net.Compilation;
    using Xunit;

    public class FieldWarningTests : ThriftCompilerTests
    {
        [Fact]
        public void FieldUsesSlist_ReportsWarning()
        {
            this.AssertCompilerReturnsWarningMessage(
@"struct User {
    slist Username
}",
CompilerMessageId.SlistDeprecated,
"Username");
        }

        [Fact]
        public void FieldIdNotSpecified_ReportsWarning()
        {
            this.AssertCompilerReturnsWarningMessage(
@"struct User {
    string $Username$
}",
CompilerMessageId.FieldIdNotSpecified,
"Username");
        }
    }
}