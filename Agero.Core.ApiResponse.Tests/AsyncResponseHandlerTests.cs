using System.Collections.ObjectModel;
using System.ComponentModel.Design.Serialization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using Agero.Core.ApiResponse.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Agero.Core.ApiResponse.Tests
{
    [TestClass]
    public class AsyncResponseHandlerTests
    {
        [TestMethod]
        public async Task HandleValidationErrorsAsync_Retruns_Response_When_there_are_errors()
        {
            //Arrange - Set HTTP Request Configuration
            var httpConfiguration = new HttpConfiguration();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/test");
            httpRequestMessage.Properties[HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
            httpRequestMessage.Content = new StringContent("String Content");
            //Arrange - Set AsyncReponseHandler Configuration
            var errors = new string[]{ "Zip must have 5 characters." };
            var asyncResponseHandler = new AsyncResponseHandler();

            //Act
            HttpResponseMessage httpResponseMessage = await asyncResponseHandler.HandleValidationErrorsAsync(httpRequestMessage, errors);
            ObjectContent<ValidationErrorResponse> objectContent = (ObjectContent<ValidationErrorResponse>) httpResponseMessage.Content;
            ValidationErrorResponse validationErrorResponse = (ValidationErrorResponse)objectContent.Value;

            //Assert
            Assert.IsNotNull(httpResponseMessage);
            Assert.AreEqual("Zip must have 5 characters.", validationErrorResponse.Errors[0]);
        }
    }
}
