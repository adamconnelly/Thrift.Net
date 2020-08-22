namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Thrift.Net.Compilation;
    using Xunit;

    public class EnumErrorTests : ThriftCompilerTests
    {
        [Fact]
        public void Compile_EnumNameMissing_ReportsError()
        {
            this.AssertCompilerReturnsError(
                "$enum$ {}",
                CompilerMessageId.EnumMustHaveAName);
        }

        [Fact]
        public void Compile_EnumMemberNameMissing_ReportsError()
        {
            this.AssertCompilerReturnsError(
@"enum UserType {
    User = 0,
    $= 1$
}",
CompilerMessageId.EnumMemberMustHaveAName);
        }

        [Fact]
        public void Compile_EnumValueNegative_ReportsError()
        {
            this.AssertCompilerReturnsError(
@"enum UserType {
    User = $-1$
}",
CompilerMessageId.EnumValueMustNotBeNegative);
        }

        [Fact]
        public void Compile_EnumValueNotAnInteger_ReportsError()
        {
            this.AssertCompilerReturnsError(
@"enum UserType {
    User = $'testing-123'$
}",
CompilerMessageId.EnumValueMustBeAnInteger);
        }

        [Fact]
        public void Compile_EnumValueMissing_ReportsError()
        {
            this.AssertCompilerReturnsError(
@"enum UserType {
    $User =$
}",
CompilerMessageId.EnumValueMustBeSpecified);
        }

        [Fact]
        public void Compile_EnumValueEqualsOperatorMissing_ReportsError()
        {
            this.AssertCompilerReturnsError(
@"enum UserType {
    $User 5$
}",
CompilerMessageId.EnumMemberEqualsOperatorMissing);
        }

        [Fact]
        public void Compile_EnumMemberDuplicated_ReportsError()
        {
            this.AssertCompilerReturnsError(
@"enum UserType {
    User,
    $User$
}",
CompilerMessageId.EnumMemberDuplicated);
        }

        [Fact]
        public void Compile_EnumMemberDuplicated_ReportsErrorMessage()
        {
            this.AssertCompilerReturnsErrorMessage(
@"enum UserType {
    User,
    User
}",
CompilerMessageId.EnumMemberDuplicated,
"User");
        }
    }
}