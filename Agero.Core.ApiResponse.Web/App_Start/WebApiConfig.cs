using System;
using System.Diagnostics;
using Agero.Core.ApiResponse.Filters;
using Agero.Core.ApiResponse.Handlers;
using System.Web.Http;
using Agero.Core.ApiResponse.Extensions;
using Newtonsoft.Json;

namespace Agero.Core.ApiResponse.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            // Adding buffering of request message content
            config.MessageHandlers.Add(new BufferingMessageContentHandler());

            // Adding response handler through ExceptionFilterAttribute 
            var responseHandler = new ResponseHandler(
                logInfo: (message, data) => Debug.WriteLine($"INFO: {message}{Environment.NewLine}{JsonConvert.SerializeObject(data)}"),
                logError: (message, data) => Debug.WriteLine($"ERROR: {message}{Environment.NewLine}{JsonConvert.SerializeObject(data)}"),
                extractAdditionalData: ex => ex.ExtractAdditionalData());

            config.Filters.Add(new ExceptionHandlingFilterAttribute(responseHandler));
        }
    }
}
