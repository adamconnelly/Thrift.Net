namespace Thrift.Net.Tests.Compilation.Types.MapType
{
    using Thrift.Net.Compilation.Types;
    using Xunit;

    public class CSharpRequiredTypeNameTests
    {
        [Fact]
        public void KeyAndValuesProvided_IncludesBothInType()
        {
            // Arrange
            var mapType = new MapType(BaseType.String, BaseType.Bool);

            // Act
            var name = mapType.CSharpRequiredTypeName;

            // Assert
            Assert.Equal("System.Collections.Generic.Dictionary<string, bool>", name);
        }

        [Fact]
        public void KeyTypeMissing_ReturnsNull()
        {
            // Arrange
            var mapType = new MapType(null, BaseType.Bool);

            // Act
            var name = mapType.CSharpRequiredTypeName;

            // Assert
            Assert.Null(name);
        }

        [Fact]
        public void ValueTypeMissing_ReturnsNull()
        {
            // Arrange
            var mapType = new MapType(BaseType.String, null);

            // Act
            var name = mapType.CSharpRequiredTypeName;

            // Assert
            Assert.Null(name);
        }
    }
}