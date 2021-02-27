using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Terminal.Common.Interfaces;
using DnD_5e.Utilities.Test;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Xunit;

namespace DnD_5e.Test.Terminal.UnitTests.Common.Interfaces
{
    public class DndApiTests
    {
        public class FreeRoll: TestBase
        {
            [Fact]
            public async Task Send_Get_Request_To_Web_Api()
            {
                var content = "{\"result\": 16, \"rolls\": [16],\"requestedRoll\": \"1d20\"}";
                var target = GivenAnApiThatReturnsSuccessfulContent(content);

                (await target.FreeRoll("1d20")).Result.Should().Be(16);
            }

            [InlineData("1d20+1", "1d20p1")]
            [InlineData("1d20-1", "1d20m1")]
            [InlineData("1d20", "1d20")]
            [Theory]
            public async Task Escapes_Plus_Minus_Before_Sending_To_Web_Api(string input, string expected)
            {
                var content = "{\"result\": 16, \"rolls\": [16],\"requestedRoll\": \"1d20\"}";
                var target = GivenAnApiThatReturnsSuccessfulContent(content, 
                    msg => msg.RequestUri.AbsoluteUri.EndsWith(expected));

                (await target.FreeRoll(input)).Result.Should().Be(16);
            }

            [Fact]
            public async Task Throws_Exception_When_Http_Exception_Code_Returned()
            {
                var target = GivenAnApiThatReturnsErrorStatusCode(HttpStatusCode.InternalServerError);

                (await target.Invoking(x => x.FreeRoll("1d20")).Should().ThrowAsync<ApiException>())
                    .WithMessage("The D&D service encountered an error processing your roll request.");
            }

            [Fact]
            public async Task Throws_Specific_Exception_For_400_Bad_Request()
            {
                var target = GivenAnApiThatReturnsErrorStatusCode(HttpStatusCode.BadRequest);

                (await target.Invoking(x => x.FreeRoll("1d20")).Should().ThrowAsync<ApiException>())
                    .WithMessage("Your roll request does not appear to be properly formatted. Please try again.");
            }

            [Fact]
            public async Task Throws_Exception_With_Inner_Exception_For_Unexpected_Error_Code()
            {
                var target = GivenAnApiThatReturnsErrorStatusCode(HttpStatusCode.Gone);

                (await target.Invoking(x => x.FreeRoll("1d20")).Should().ThrowAsync<ApiException>())
                    .WithInnerException<HttpRequestException>().Which.StatusCode.Should().Be(HttpStatusCode.Gone);
            }

            private DndApi GivenAnApiThatReturnsSuccessfulContent(string content, Expression<Func<HttpRequestMessage, bool>> filter = null)
            {
                return GivenAnApiThatReturnsExpectedResponse(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(content)
                }, filter);
            }

            private DndApi GivenAnApiThatReturnsErrorStatusCode(HttpStatusCode statusCode)
            {
                return GivenAnApiThatReturnsExpectedResponse(new HttpResponseMessage(statusCode));
            }

            private DndApi GivenAnApiThatReturnsExpectedResponse(HttpResponseMessage expectedResponse, Expression<Func<HttpRequestMessage, bool>> filter = null)
            {
                var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
                mockHandler.Protected().Setup<Task<HttpResponseMessage>>(
                        "SendAsync", filter == null ? ItExpr.IsAny<HttpRequestMessage>() : ItExpr.Is(filter),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(expectedResponse);
                Mocker.Use(mockHandler);
                Mocker.Use(new HttpClient(mockHandler.Object) { BaseAddress = new Uri("http://www.dndapi.com/") });
                var target = Mocker.CreateInstance<DndApi>();
                return target;
            }
        }
    }
}
