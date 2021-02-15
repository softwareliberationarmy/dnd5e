using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Terminal.Common;
using DnD_5e.Terminal.Common.Interfaces;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Moq.Protected;
using Xunit;
namespace DnD_5e.Terminal.Test.UnitTests
{
    public class DndApiTests
    {
        public class FreeRoll
        {
            [Fact]
            public async Task Send_Get_Request_To_Web_Api()
            {
                var mocker = new AutoMocker();
                var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
                mockHandler.Protected().Setup<Task<HttpResponseMessage>>(
                        "SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("{\"result\": 16, \"rolls\": [16],\"requestedRoll\": \"1d20\"}")
                    });
                mocker.Use(mockHandler);
                mocker.Use(new HttpClient(mockHandler.Object){ BaseAddress = new Uri("http://www.dndapi.com/")});
                var target = mocker.CreateInstance<DndApi>();

                (await target.FreeRoll("1d20")).Result.Should().Be(16);
            }

            [InlineData("1d20+1", "1d20p1")]
            [InlineData("1d20-1", "1d20m1")]
            [InlineData("1d20", "1d20")]
            [Theory]
            public async Task Escapes_Plus_Minus_Before_Sending_To_Web_Api(string input, string expected)
            {
                var mocker = new AutoMocker();
                var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
                mockHandler.Protected().Setup<Task<HttpResponseMessage>>(
                        "SendAsync", ItExpr.Is<HttpRequestMessage>(msg => msg.RequestUri.AbsoluteUri.EndsWith(expected)),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("{\"result\": 16, \"rolls\": [16],\"requestedRoll\": \"1d20\"}")
                    });
                mocker.Use(mockHandler);
                mocker.Use(new HttpClient(mockHandler.Object) { BaseAddress = new Uri("http://www.dndapi.com/") });
                var target = mocker.CreateInstance<DndApi>();

                (await target.FreeRoll(input)).Result.Should().Be(16);
            }


            [Fact]
            public async Task Throws_Exception_When_Http_Exception_Code_Returned()
            {
                var mocker = new AutoMocker();
                var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
                mockHandler.Protected().Setup<Task<HttpResponseMessage>>(
                        "SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                mocker.Use(mockHandler);
                mocker.Use(new HttpClient(mockHandler.Object) { BaseAddress = new Uri("http://www.dndapi.com/") });
                var target = mocker.CreateInstance<DndApi>();

                await target.Invoking(x => x.FreeRoll("1d20")).Should().ThrowAsync<ApiException>();
            }
        }
    }
}
