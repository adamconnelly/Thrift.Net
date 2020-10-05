namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Thrift.Net.Compilation;
    using Xunit;

    public class EnumErrorTests : ThriftCompilerTests
    {
        [Fact]
        public void Compile_EnumNameMissing_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
                "$enum$ {}",
                CompilerMessageId.EnumMustHaveAName);
        }

        [Fact]
        public void Compile_EnumMemberNameMissing_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"enum UserType {
    User = 0,
    $= 1$
}",
CompilerMessageId.EnumMemberMustHaveAName);
        }

        [Fact]
        public void Compile_EnumValueNegative_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"enum UserType {
    User = $-1$
}",
CompilerMessageId.EnumValueMustNotBeNegative);
        }

        [Fact]
        public void Compile_EnumValueNotAnInteger_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"enum UserType {
    User = $'testing-123'$
}",
CompilerMessageId.EnumValueMustBeAnInteger);
        }

        [Fact]
        public void Compile_EnumValueMissing_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"enum UserType {
    $User =$
}",
CompilerMessageId.EnumValueMustBeSpecified);
        }

        [Fact]
        public void Compile_EnumValueEqualsOperatorMissing_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"enum UserType {
    $User 5$
}",
CompilerMessageId.EnumMemberEqualsOperatorMissing);
        }

        [Fact]
        public void Compile_EnumMemberDuplicated_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"enum UserType {
    User,
    $User$
}",
CompilerMessageId.EnumMemberDuplicated,
"User");
        }

        [Fact]
        public void Compile_EnumDuplicated_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"enum UserType {
    User
}
enum $UserType$ {
    User
}",
CompilerMessageId.NameAlreadyDeclared,
"UserType");
        }

        [Fact]
        public void Compile_EnumValueInvalidHex_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"enum UserType {
    User = $0xZZZ$
}",
CompilerMessageId.EnumValueInvalidHex,
"0xZZZ");
        }

        [Fact]
        public void Compile_DuplicateEnumValue_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"enum UserType {
    User = 10
    Administrator = $10$
}",
CompilerMessageId.EnumValueDuplicated,
"10");
        }

        [Fact]
        public void Compile_EnumValueDuplicatedWithHexValue_ReportsHexValueInErrorMessage()
        {
            this.AssertCompilerReturnsErrorMessage(
@"enum UserType {
    User = 1
    Administrator = $0x01$
}",
CompilerMessageId.EnumValueDuplicated,
"0x01");
        }
    }
}