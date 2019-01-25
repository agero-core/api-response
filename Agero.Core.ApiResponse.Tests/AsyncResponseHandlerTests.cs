using System;
using Agero.Core.ApiResponse.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using Agero.Core.ApiResponse.Exceptions;

namespace Agero.Core.ApiResponse.Tests
{
    [TestClass]
    public class AsyncResponseHandlerTests
    {
        [TestMethod]
        public async Task HandleValidationErrorsAsync_Returns_HttpResponseMessage_When_there_are_errors()
        {
            //Arrange - Set HTTP Request Configuration
            var httpConfiguration = new HttpConfiguration();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/test");
            httpRequestMessage.Properties[HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
            httpRequestMessage.Content = new StringContent("Test String Content");
            //Arrange - Set AsyncReponseHandler Configuration
            var errors = new string[]{ "Zip must have 5 characters." };
            var asyncResponseHandler = new AsyncResponseHandler();

            //Act
            HttpResponseMessage httpResponseMessage = await asyncResponseHandler.HandleValidationErrorsAsync(httpRequestMessage, errors);
            ObjectContent<ValidationErrorResponse> objectContent = (ObjectContent<ValidationErrorResponse>) httpResponseMessage.Content;
            ValidationErrorResponse validationErrorResponse = (ValidationErrorResponse) objectContent.Value;

            //Assert
            Assert.IsNotNull(httpResponseMessage);
            Assert.AreEqual("Zip must have 5 characters.", validationErrorResponse.Errors[0]);
        }

        [TestMethod]
        public async Task HandleExceptionAsync_Returns_BaseExceptionResponse_When_there_is_a_BadRequestException()
        {
            //Arrange - Set HTTP Request Configuration
            var httpConfiguration = new HttpConfiguration();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/test");
            httpRequestMessage.Properties[HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
            httpRequestMessage.Content = new StringContent("Test String Content");
            //Arrange - Set AsyncReponseHandler Configuration
            var asyncResponseHandler = new AsyncResponseHandler();

            //Act
            HttpResponseMessage httpResponseMessage = await asyncResponseHandler.HandleExceptionAsync(httpRequestMessage, new BadRequestException("Bad Request Exception"));
            ObjectContent<InfoResponse> objectContent = (ObjectContent<InfoResponse>) httpResponseMessage.Content;
            InfoResponse infoResponse = (InfoResponse) objectContent.Value;

            //Assert
            Assert.IsNotNull(httpResponseMessage);
            Assert.AreEqual("Bad Request Exception", infoResponse.Message);
        }

        [TestMethod]
        public async Task HandleExceptionAsync_Returns_UnexpectedExceptionResponse_When_there_is_a_UnexpectedException()
        {
            //Arrange - Set HTTP Request Configuration
            var httpConfiguration = new HttpConfiguration();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/test");
            httpRequestMessage.Properties[HttpPropertyKeys.HttpConfigurationKey] = httpConfiguration;
            httpRequestMessage.Content = new StringContent("Test String Content");
            //Arrange - Set AsyncReponseHandler Configuration
            var asyncResponseHandler = new AsyncResponseHandler();

            //Act
            HttpResponseMessage httpResponseMessage = await asyncResponseHandler.HandleExceptionAsync(httpRequestMessage, new Exception("Unexpected Exception"));
            ObjectContent<InfoResponse> objectContent = (ObjectContent<InfoResponse>) httpResponseMessage.Content;
            ErrorResponse errorResponse = (ErrorResponse) objectContent.Value;

            //Assert
            Assert.IsNotNull(httpResponseMessage);
            Assert.AreEqual("Unexpected Exception", errorResponse.Error.Message);
        }
    }
}
