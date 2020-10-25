namespace Thrift.Net.Tests.Compilation.ThriftCompiler
{
    using Xunit;

    public class CommentTests : ThriftCompilerTests
    {
        [Fact]
        public void SupportSingleLineCppStyleComments()
        {
            var input =
@"// Represents a User
struct User {
    1: i32 Id
}";
            this.AssertCompilerDoesNotReturnAnyMessages(input);
        }

        [Fact]
        public void SupportSingleLineShellStyleComments()
        {
            var input =
@"# Represents a User
struct User {
    1: i32 Id
}";
            this.AssertCompilerDoesNotReturnAnyMessages(input);
        }

        [Fact]
        public void SupportMultiLineCStyleComments()
        {
            var input =
@"/*
 * Represents a user.
 */
struct User {
    1: i32 Id
}";
            this.AssertCompilerDoesNotReturnAnyMessages(input);
        }
    }
}