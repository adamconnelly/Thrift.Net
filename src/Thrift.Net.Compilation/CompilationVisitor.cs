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

        // Used to store the current value of an enum so we can automatically generate
        // values if they aren't defined explicitly.
        private readonly ParseTreeProperty<int> currentEnumValue = new ParseTreeProperty<int>();

        /// <summary>
        /// Gets the enums defined in the document.
        /// </summary>
        public IReadOnlyCollection<EnumDefinition> Enums => this.enums;

        /// <inheritdoc />
        public override int? VisitEnumDefinition(ThriftParser.EnumDefinitionContext context)
        {
            this.currentEnumValue.Put(context, 0);

            var result = base.VisitEnumDefinition(context);

            this.currentEnumValue.RemoveFrom(context);

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
            var value = this.GetEnumValue(context);

            var enumMember = new EnumMember(name, value);

            this.enumMembers.Put(context, enumMember);

            return result;
        }

        private int GetEnumValue(ThriftParser.EnumMemberContext context)
        {
            // According to the Thrift IDL specification, if an enum value is
            // not supplied, it should either be:
            //   - 0 for the first element in an enum.
            //   - P+1 - where `P` is the value of the previous element.
            var currentValue = this.currentEnumValue.Get(context.Parent);
            if (context.INT_CONSTANT() != null)
            {
                currentValue = int.Parse(context.INT_CONSTANT().Symbol.Text);
            }

            this.currentEnumValue.Put(context.Parent, currentValue + 1);

            return currentValue;
        }
    }
}