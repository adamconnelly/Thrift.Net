namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Thrift.Net.Compilation;
    using Xunit;

    public class ConstantErrorTests : ThriftCompilerTests
    {
        [Fact]
        public void ConstantNameMissing_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
                "$const i32 = 100$",
                CompilerMessageId.ConstantMustHaveAName);
        }

        [Fact]
        public void ConstantExpressionMissing_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
                "const i32 $MaxPageSize$",
                CompilerMessageId.ConstantMustBeInitialized,
                "MaxPageSize");
        }

        [Fact]
        public void ConstantExpressionMissingButHasEqualsOperator_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
                "const i32 $MaxPageSize$ =",
                CompilerMessageId.ConstantMustBeInitialized,
                "MaxPageSize");
        }

        [Fact]
        public void ConstantExpressionMissingEqualsOperator_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
                "const i32 $MaxPageSize$ 100",
                CompilerMessageId.ConstantMissingEqualsOperator,
                "MaxPageSize");
        }

        [Theory]
        [InlineData("i8", "128")]
        [InlineData("i8", "-129")]
        [InlineData("i16", "32768")]
        [InlineData("i16", "-32769")]
        [InlineData("i32", "2147483648")]
        [InlineData("i32", "-2147483649")]
        [InlineData("i8", "1.5")]
        [InlineData("i16", "1.5")]
        [InlineData("i32", "1.5")]
        [InlineData("i64", "1.5")]
        [InlineData("i8", "\"test\"")]
        [InlineData("i16", "\"test\"")]
        [InlineData("i32", "\"test\"")]
        [InlineData("i64", "\"test\"")]
        [InlineData("double", "\"test\"")]
        public void ExpressionTypeDoesNotMatchConstantType_ReportsError(string constantType, string expression)
        {
            this.AssertCompilerReturnsErrorMessage(
                $"const {constantType} TestConstant = ${expression}$",
                CompilerMessageId.ConstantExpressionCannotBeAssignedToType,
                expression,
                constantType);
        }
    }
}
