namespace Thrift.Net.Tests.Compilation.Binding.DocumentBinder
{
    using NSubstitute;
    using Thrift.Net.Compilation.Binding;

    // TODO: Need some tests for the document binder
    public abstract class DocumentBinderTests
    {
        private readonly IBinderProvider binderProvider = Substitute.For<IBinderProvider>();
        private readonly DocumentBinder binder;

        public DocumentBinderTests()
        {
            this.binder = new DocumentBinder(this.binderProvider);
        }

        protected IBinderProvider BinderProvider => this.binderProvider;

        protected DocumentBinder Binder => this.binder;
    }
}