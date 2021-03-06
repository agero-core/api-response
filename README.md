# API Response

API Response is a **.NET Framework (>= v4.6.1)** and **.NET Core (>=2.1)** library for API response handling and logging.

## Setup (.NET Framework):

Create and setup an instance of [IResponseHandler](./Agero.Core.ApiResponse/IResponseHandler.cs)
```csharp
var responseHandler = new ResponseHandler(
	// Setup info logging 
	logInfo: (message, data) => Debug.WriteLine($"INFO: {message}{Environment.NewLine}{JsonConvert.SerializeObject(data)}"),
	// Setup error logging 
	logError: (message, data) => Debug.WriteLine($"ERROR: {message}{Environment.NewLine}{JsonConvert.SerializeObject(data)}"),
	// For logging, setup a way to extract additional data from exceptions
	extractAdditionalData: ex => ex.ExtractAdditionalData(),
	// Define whether exception details should be included in HTTP response
	includeExceptionDetails: true);
```

Register an instance of [ExceptionHandlingFilterAttribute](./Agero.Core.ApiResponse/Filters/ExceptionHandlingFilterAttribute.cs) in [HttpConfiguration](https://docs.microsoft.com/en-us/previous-versions/aspnet/hh833997(v=vs.118)):
```csharp
config.Filters.Add(new ExceptionHandlingFilterAttribute(responseHandler));
```

Ensure that request body can be read multiple times by registering [BufferingMessageContentHandler](./Agero.Core.ApiResponse/Handlers/BufferingMessageContentHandler.cs): 
```csharp
httpConfig.MessageHandlers.Add(new BufferingMessageContentHandler());
```

## Setup (.NET Core):

Create and setup an instance of [IResponseHandler](./Agero.Core.ApiResponse/IResponseHandler.cs)
```csharp
var responseHandler = new ResponseHandler(
	// Setup info logging 
	logInfo: (message, data) => Debug.WriteLine($"INFO: {message}{Environment.NewLine}{JsonConvert.SerializeObject(data)}"),
	// Setup error logging 
	logError: (message, data) => Debug.WriteLine($"ERROR: {message}{Environment.NewLine}{JsonConvert.SerializeObject(data)}"),
	// For logging, setup a way to extract additional data from exceptions
	extractAdditionalData: ex => ex.ExtractAdditionalData(),
	// Define whether exception details should be included in HTTP response
	includeExceptionDetails: true);
```

Register an instance of [ExceptionHandlingFilterAttribute](./Agero.Core.ApiResponse/Filters/ExceptionHandlingFilterAttribute.cs) in [MvcOptions](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.mvcoptions):
```csharp
public void ConfigureServices(IServiceCollection services)
{
	services.AddMvc(config =>
	{
		config.Filters.Add(new ExceptionHandlingFilterAttribute(responseHandler));
	});
}
```

Ensure that request body can be read multiple times by calling [EnableBuffering](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httprequestrewindextensions.enablebuffering) method: 
```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
	app.Use(requestDelegate => context =>
	{
		context.Request.EnableBuffering();

		return requestDelegate(context);
	});
}
```

## Usage (common exception):

Throw an exception during request:
```csharp
throw new Exception("Application error.");
```
and API will return the following response with 500 (Internal Server Error) HTTP status:
```json
{
    "error": {
        "message": "Application error.",
        "type": "System.Exception",
        "stackTrace": "...",
        "innerError": null,
        "data": null
    },
    "message": "Unexpected error",
    "code": "UNEXPECTED_ERROR"
}
```
and it will create the following log:
```json
{  
   "request":{  
      "method":"GET",
      "url":"http://localhost:64272/responses/applicationError",
      "body":"",
      "headers":[ ]
   },
   "response":{  
      "status":500,
      "code":"UNEXPECTED_ERROR",
      "exception":{  
         "message":"Application error.",
         "type":"System.Exception",
         "stackTrace":"...",
         "innerError":null,
         "data":null
      }
   }
}
```

## Usage (detailed exception):

Throw an exception inherited from [BaseException](./Agero.Core.ApiResponse/Exceptions/BaseException.cs) during request, which allows to customize HTTP status and add additional data to log:
```csharp
throw new BadRequestException(
	message:"Validation error.", 
	code: "VALIDATION_ERROR", 
	additionalData: new { status = "validation_error" });
```
and API will return the following response with 400 (Bad Request) HTTP status:
```json
{
    "message": "Validation error.",
    "code": "VALIDATION_ERROR"
}
```
and it will create the following log:
```json
{  
   "request":{  
      "method":"GET",
      "url":"http://localhost:64272/responses/validationError",
      "body":"",
      "headers":[  ]
   },
   "response":{  
      "status":400,
      "code":"VALIDATION_ERROR",
      "exception":{  
         "message":"Validation error.",
         "type":"Agero.Core.ApiResponse.Exceptions.BadRequestException",
         "stackTrace":"...",
         "innerError":null,
         "data":{  
            "status":"validation_error"
         }
      }
   }
}
```

For additional usage related info please see [Agero.Core.ApiResponse.Web](./Agero.Core.ApiResponse.Web/) (.NET Framework) and [Agero.Core.ApiResponse.Web.Core](./Agero.Core.ApiResponse.Web.Core/) (.NET Core).