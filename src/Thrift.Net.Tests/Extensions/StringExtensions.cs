namespace Thrift.Net.Tests.Extensions
{
    using System.IO;
    using System.Text;

    public static class StringExtensions
    {
        public static Stream ToStream(this string input)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(input));
        }
    }
}