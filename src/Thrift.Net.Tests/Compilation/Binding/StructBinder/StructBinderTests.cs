namespace Thrift.Net.Tests.Compilation.Binding.StructBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;

    public abstract class StructBinderTests
    {
        public StructBinderTests()
        {
            this.Binder = new StructBinder(this.Parent, this.BinderProvider);
        }

        protected IBinder Parent { get; } = Substitute.For<IBinder>();

        protected IBinderProvider BinderProvider { get; } = Substitute.For<IBinderProvider>();

        protected StructBinder Binder { get; }
    }
}