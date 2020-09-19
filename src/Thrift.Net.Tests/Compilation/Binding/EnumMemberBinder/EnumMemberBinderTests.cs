namespace Thrift.Net.Tests.Compilation.Binding.EnumMemberBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;

    public abstract class EnumMemberBinderTests
    {
        private readonly IEnumBinder parentBinder = Substitute.For<IEnumBinder>();
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();
        private readonly EnumMemberBinder binder;

        public EnumMemberBinderTests()
        {
            this.binder = new EnumMemberBinder(this.parentBinder, this.binderProvider);
        }

        public IEnumBinder ParentBinder => this.parentBinder;

        public EnumMemberBinder Binder => this.binder;
    }
}