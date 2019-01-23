using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Agero.Core.ApiResponse.Handlers
{
    /// <summary>Handlers which enables reading request's content more than one time</summary>
    public class BufferingMessageContentHandler : DelegatingHandler
    {
        /// <summary>Sends an HTTP request to the inner handler to send to the server as an asynchronous operation</summary>
        /// <param name="request">HTTP request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Content is a stream that can be read only once. To read it many times we need to buffer it before anybody requests it.
            await request.Content.LoadIntoBufferAsync();

            return await base.SendAsync(request, cancellationToken);
        }
    }
}

