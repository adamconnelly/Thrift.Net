namespace Thrift.Net.Tests.Compilation.Types.MapType
{
    using Thrift.Net.Compilation.Types;
    using Xunit;

    public class NameTests
    {
        [Fact]
        public void KeyAndValuesProvided_IncludesBothInName()
        {
            // Arrange
            var mapType = new MapType(BaseType.String, BaseType.Bool);

            // Act
            var name = mapType.Name;

            // Assert
            Assert.Equal("map<string, bool>", name);
        }

        [Fact]
        public void KeyNotProvided_DoesNotIncludeInName()
        {
            // Arrange
            var mapType = new MapType(null, BaseType.Bool);

            // Act
            var name = mapType.Name;

            // Assert
            Assert.Equal("map<,bool>", name);
        }

        [Fact]
        public void ValueNotProvided_DoesNotIncludeInName()
        {
            // Arrange
            var mapType = new MapType(BaseType.String, null);

            // Act
            var name = mapType.Name;

            // Assert
            Assert.Equal("map<string,>", name);
        }
    }
}