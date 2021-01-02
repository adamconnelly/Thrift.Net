namespace Thrift.Net.Compilation
{
    using System.IO;
    using Antlr4.StringTemplate;
    using Thrift.Net.Compilation.Symbols;

    /// <summary>
    /// Used to generate C# from Thrift document models.
    /// </summary>
    public class ThriftDocumentGenerator : IThriftDocumentGenerator
    {
        /// <inheritdoc/>
        public string Generate(ThriftFile file, IDocument document)
        {
            var assembly = typeof(ThriftDocumentGenerator).Assembly;
            var rawTemplate = assembly.GetManifestResourceStream(
                $"{assembly.GetName().Name}.Templates.csharp.stg");
            using var reader = new StreamReader(rawTemplate);
            var templateGroup = new TemplateGroupString(
                "csharp.stg",
                reader.ReadToEnd(),
                '$',
                '$');
            var template = templateGroup.GetInstanceOf("document");

            // TODO: Get the semantic version
            template.Add("version", assembly.GetName().Version.ToString())
                .Add("file", file)
                .Add("model", document)
                .Add("typeMap", ThriftTypeGenerationInfo.TypeMap);

            return template.Render();
        }
    }
}