namespace Thrift.Net.Tests.Compilation.Binding.EnumBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;

    public abstract class EnumBinderTests
    {
        private readonly IBinder parentBinder = Substitute.For<IBinder>();
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();
        private readonly EnumBinder binder;

        public EnumBinderTests()
        {
            this.binder = new EnumBinder(this.parentBinder, this.binderProvider);
        }

        public IBinder ParentBinder => this.parentBinder;

        public IBinderProvider BinderProvider => this.binderProvider;

        public EnumBinder Binder => this.binder;
    }
}