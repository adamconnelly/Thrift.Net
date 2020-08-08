namespace Thrift.Net.Compilation
{
    using System.Collections.Generic;
    using System.Linq;
    using Antlr4.Runtime.Tree;
    using Thrift.Net.Antlr;
    using Thrift.Net.Compilation.Model;

    /// <summary>
    /// A visitor used to perform the main compilation.
    /// </summary>
    public class CompilationVisitor : ThriftBaseVisitor<int?>
    {
        private readonly List<EnumDefinition> enums = new List<EnumDefinition>();
        private readonly ParseTreeProperty<EnumMember> enumMembers = new ParseTreeProperty<EnumMember>();

        /// <summary>
        /// Gets the enums defined in the document.
        /// </summary>
        public IReadOnlyCollection<EnumDefinition> Enums => this.enums;

        /// <inheritdoc />
        public override int? VisitEnumDefinition(ThriftParser.EnumDefinitionContext context)
        {
            var result = base.VisitEnumDefinition(context);

            var name = context.IDENTIFIER().Symbol.Text;
            var members = context.enumMember().Select(member => this.enumMembers.Get(member));

            this.enums.Add(new EnumDefinition(name, members.ToList()));

            return result;
        }

        /// <inheritdoc />
        public override int? VisitEnumMember(ThriftParser.EnumMemberContext context)
        {
            var result = base.VisitEnumMember(context);

            var name = context.IDENTIFIER().Symbol.Text;
            var value = GetEnumValue(context.INT_CONSTANT());

            var enumMember = new EnumMember(name, value);

            this.enumMembers.Put(context, enumMember);

            return result;
        }

        private static int GetEnumValue(ITerminalNode constant)
        {
            if (constant != null)
            {
                return int.Parse(constant.Symbol.Text);
            }

            return 0;
        }
    }
}