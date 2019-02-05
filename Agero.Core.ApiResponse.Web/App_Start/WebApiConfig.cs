using Agero.Core.ApiResponse.Filters;
using Agero.Core.ApiResponse.Handlers;
using System.Web.Http;

namespace Agero.Core.ApiResponse.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.MessageHandlers.Add(DIContainer.Instance.CreateInstance<BufferingMessageContentHandler>());

            config.Filters.Add(new ExceptionHandlingFilterAttribute(DIContainer.Instance.Get<IResponseHandler>()));
        }
    }
}
