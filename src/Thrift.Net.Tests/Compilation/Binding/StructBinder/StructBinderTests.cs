namespace Thrift.Net.Tests.Compilation.Binding.StructBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;

    public abstract class StructBinderTests
    {
        public StructBinderTests()
        {
            this.Binder = new StructBinder(this.BinderProvider);
        }

        protected IBinderProvider BinderProvider { get; } = Substitute.For<IBinderProvider>();

        protected StructBinder Binder { get; }
    }
}