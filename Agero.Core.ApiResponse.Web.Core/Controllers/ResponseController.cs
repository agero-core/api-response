using Microsoft.AspNetCore.Mvc;
using System;
using Agero.Core.ApiResponse.Exceptions;

namespace Agero.Core.ApiResponse.Web.Core.Controllers
{
    [Route("responses")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        [Route("applicationError")]
        [HttpGet]
        public ObjectResult GetApplicationErrorResponse()
        {
            // See debug logs
            throw new Exception("Application error.");
        }

        [Route("validationError")]
        [HttpGet]
        public ObjectResult GetValidationErrorResponse()
        {
            // See debug logs
            throw new BadRequestException(
                message: "Validation error.",
                code: "VALIDATION_ERROR",
                additionalData: new { status = "validation_error" });
        }
    }
}
