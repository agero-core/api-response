using Agero.Core.ApiResponse.Extensions;
using Agero.Core.DIContainer;
using Agero.Core.Lazy;
using System.Threading.Tasks;

namespace Agero.Core.ApiResponse.Web
{
    public class DIContainer
    {
        private static readonly SyncLazy<IContainer> _container =
            new SyncLazy<IContainer>(CreateContainer);

        private static IContainer CreateContainer()
        {
            var container = ContainerFactory.Create();

            container.RegisterFactoryMethod<IResponseHandler>(c => new AsyncResponseHandler(
                logInfoAsync: async (message, obj) => await Task.FromResult(0),
                logErrorAsync: async (message, obj) => await Task.FromResult(0),
                extractAdditionalData: (ex => ex.ExtractAdditionalData()),
                includeExceptionDetails: true
                ), Lifetime.PerContainer);

            return container;
        }

        public static IContainer Instance => _container.Value;
    }
}