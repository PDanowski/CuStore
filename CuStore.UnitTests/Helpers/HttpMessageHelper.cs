using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace CuStore.UnitTests.Helpers
{
    internal class HttpMessageHelper
    {
        internal Mock<HttpMessageHandler> CreateMock(string responseContent)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Default);
            handlerMock.Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                })
                .Verifiable();

            return handlerMock;
        }
    }
}
