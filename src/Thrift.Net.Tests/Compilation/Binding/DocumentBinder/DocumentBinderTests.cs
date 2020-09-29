namespace Thrift.Net.Tests.Compilation.Binding.DocumentBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;

    // TODO: Need some tests for the document binder
    public abstract class DocumentBinderTests
    {
        private readonly IBinder parentBinder = Substitute.For<IBinder>();
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();
        private readonly DocumentBinder binder;

        public DocumentBinderTests()
        {
            this.binder = new DocumentBinder(this.parentBinder, this.binderProvider);
        }

        protected IBinder ParentBinder => this.parentBinder;

        protected IBinderProvider BinderProvider => this.binderProvider;

        protected DocumentBinder Binder => this.binder;
    }
}