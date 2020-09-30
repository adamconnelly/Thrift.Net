namespace Thrift.Net.Tests.Compilation.Binding.EnumMemberBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;

    public abstract class EnumMemberBinderTests
    {
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();
        private readonly EnumMemberBinder binder;

        public EnumMemberBinderTests()
        {
            this.binder = new EnumMemberBinder(this.binderProvider);
        }

        public EnumMemberBinder Binder => this.binder;
    }
}