namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Thrift.Net.Compilation;
    using Xunit;

    public class StructErrorTests : ThriftCompilerTests
    {
        [Fact]
        public void Compile_StructNameMissing_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
                "$struct$ {}",
                CompilerMessageId.StructMustHaveAName);
        }

        [Fact]
        public void Compile_StructNameAlreadyUsed_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"enum User {}
struct $User$ {}",
CompilerMessageId.NameAlreadyDeclared,
"User");
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

        [Fact]
        public void Compile_FieldIdZero_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"struct User {
    $0$: string Username
}",
CompilerMessageId.StructFieldIdMustBeAPositiveInteger,
"0");
        }

        [Fact]
        public void Compile_FieldTypeCannotBeResolved_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"struct User {
    1: $UserType$ Username
}",
CompilerMessageId.UnknownType,
"UserType");
        }

        [Theory]
        [InlineData("$list$")]
        [InlineData("$list<>$")]
        public void Compile_ListElementTypeNotSpecified_ReportsError(string scenario)
        {
            this.AssertCompilerReturnsErrorMessage(
$@"struct User {{
    1: {scenario} Emails
}}",
CompilerMessageId.ListMustHaveElementTypeSpecified);
        }

        [Fact]
        public void Compile_NestedListElementTypeNotSpecified_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"struct User {
    1: list<$list<>$> Addresses
}",
CompilerMessageId.ListMustHaveElementTypeSpecified);
        }

        [Fact]
        public void Compile_ListElementUnresolved_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"struct User {
    1: list<$PermissionType$> Permissions
}",
CompilerMessageId.UnknownType,
"PermissionType");
        }

        [Theory]
        [InlineData("$set$")]
        [InlineData("$set<>$")]
        public void Compile_SetElementTypeNotSpecified_ReportsError(string scenario)
        {
            this.AssertCompilerReturnsErrorMessage(
$@"struct User {{
    1: {scenario} Emails
}}",
CompilerMessageId.SetMustHaveElementTypeSpecified);
        }

        [Fact]
        public void Compile_NestedSetElementTypeNotSpecified_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"struct User {
    1: set<$set<>$> Emails
}",
CompilerMessageId.SetMustHaveElementTypeSpecified);
        }

        [Fact]
        public void Compile_MapKeyTypeNotSpecified_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"struct User {
    1: $map<, string>$ Emails
}",
CompilerMessageId.MapMustHaveKeyTypeSpecified);
        }

        [Fact]
        public void Compile_NestedMapKeyTypeNotSpecified_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"struct User {
    1: map<$map<, string>$, bool> Emails
}",
CompilerMessageId.MapMustHaveKeyTypeSpecified);
        }

        [Fact]
        public void Compile_MapValueTypeNotSpecified_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"struct User {
    1: $map<EmailType, >$ Emails
}",
CompilerMessageId.MapMustHaveValueTypeSpecified);
        }

        [Fact]
        public void Compile_NestedMapValueTypeNotSpecified_ReportsError()
        {
            this.AssertCompilerReturnsErrorMessage(
@"struct User {
    1: map<bool, $map<EmailType, >$> Emails
}",
CompilerMessageId.MapMustHaveValueTypeSpecified);
        }

        [Theory]
        [InlineData("$map<>$")]
        [InlineData("$map<,>$")]
        [InlineData("$map$")]
        public void Compile_MapKeyAndValueTypeNotSpecified_ReportsError(string scenario)
        {
            this.AssertCompilerReturnsErrorMessage(
$@"struct User {{
    1: {scenario} Emails
}}",
CompilerMessageId.MapMustHaveKeyAndValueTypeSpecified);
        }
    }
}