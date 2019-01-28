using Agero.Core.DIContainer;
using Agero.Core.Lazy;

namespace Agero.Core.ApiResponse.Web
{
    public class DIContainer
    {
        private static readonly SyncLazy<IContainer> _container =
            new SyncLazy<IContainer>(CreateContainer);

        private static IContainer CreateContainer()
        {
            var container = ContainerFactory.Create();

            container.RegisterFactoryMethod<IResponseHandler>(c => new AsyncResponseHandler(), Lifetime.PerContainer);

            return container;
        }

        public static IContainer Instance => _container.Value;
    }
}