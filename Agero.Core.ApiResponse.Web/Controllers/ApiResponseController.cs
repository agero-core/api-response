using Agero.Core.ApiResponse.Web.Models;
using Agero.Core.Validator;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agero.Core.ApiResponse.Web.Controllers
{
    [RoutePrefix("apiresponse")]
    public class ApiResponseController : ApiController
    {
        [Route("errors")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetErrorResponse()
        {
            var asyncResponseHandler = DIContainer.Instance.Get<IResponseHandler>();

            return await asyncResponseHandler.HandleValidationErrorsAsync(Request, new string[] {"This is a sample test error."});
        }

        [Route("exception")]
        [HttpPost]
        public async Task<string> GetValidationExceptionResponse([FromBody] Name request)
        {
            var validator = new ValidationHelper();

            validator.CheckIsValid(request);

            return await Task.FromResult("Check for Exception Filter execution.");
        }
    }
}