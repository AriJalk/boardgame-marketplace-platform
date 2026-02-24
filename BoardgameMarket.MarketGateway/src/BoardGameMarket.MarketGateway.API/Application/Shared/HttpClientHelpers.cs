using System.Text;
using System.Text.Json;

namespace BoardGameMarket.MarketGateway.API.Application.Shared
{
    public static class HttpClientHelpers
    {

        public static HttpRequestMessage BuildRequest<T>(HttpMethod httpMethod, string uri, Guid requesterUserId, T content)
        {
            HttpRequestMessage requestMessage = BuildRequest(httpMethod, uri, requesterUserId);

            string json = JsonSerializer.Serialize(content);
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return requestMessage;
        }

        public static HttpRequestMessage BuildRequest(HttpMethod httpMethod, string uri, Guid requesterUserId)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod, uri);
            requestMessage.Headers.Add("X-Requester-UserId", requesterUserId.ToString());

            return requestMessage;
        }
    }
}
