# Api Response

Api Resonse is a **.NET Framework** library for convenient handling of responses in API's.

## Usage:

API Resonse library provides an implementation of DelegatingHandler, **BufferingMessageContentHandler**, which intercepts the HTTP request made to the server and helps keep the content of the http request in the buffer. Since, the content is a stream that can be read only once, to read it many times we need to buffer it before anybody requests it. To use **BufferingMessageContentHandler**, register an instance with the **HttpConfiguration**.

```csharp
httpConfig.MessageHandlers.Add(new BufferingMessageContentHandler());
```

API Response library provides two implementations of IResponseHandler, **AsyncResponseHandler** and **ResponseHandler**. Below example uses **AsyncResponseHandler**.

```csharp
IResponseHandler asyncResponseHandler = 
    new AsyncResponseHandler(
        //optional method to handle information logging
        logInfoAsync: async (message, obj) => await Task.FromResult(0),
        //optional method to handle logging errors
        logErrorAsync: async (message, obj) => await Task.FromResult(0),
        //optional method that extracts exception additional data
        extractAdditionalData : (ex => ex.ExtractAdditionalData()),
        //optinal: By default set to true
        includeExceptionDetails : false
);
```

Api Response library provides an implementation of ExceptionFilterAttribute, **ExceptionHandlingFilterAttribute**, which gets triggered when an exception is thrown during incoming HTTP request processing. To use **ExceptionHandlingFilterAttribute**, register an instance with the **HttpConfiguration**.

```csharp
httpConfig.Filters.Add(new ExceptionHandlingFilterAttribute(asyncResponsehandler));
```

For additional usage related info please see [Agero.Core.ApiResponse.Web](./Agero.Core.ApiResponse.Web/).