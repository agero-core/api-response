using System;
using System.Net.Http;
using System.Web.Http;
using Agero.Core.ApiResponse.Exceptions;

namespace Agero.Core.ApiResponse.Web.Controllers
{
    [RoutePrefix("responses")]
    public class ResponseController : ApiController
    {
        [Route("applicationError")]
        [HttpGet]
        public HttpResponseMessage GetApplicationErrorResponse()
        {
            // See debug logs
            throw new Exception("Application error.");
        }

        [Route("validationError")]
        [HttpGet]
        public string GetValidationErrorResponse()
        {
            // See debug logs
            throw new BadRequestException(
                message:"Validation error.", 
                code: "VALIDATION_ERROR", 
                additionalData: new { status = "validation_error" });
        }
    }
}