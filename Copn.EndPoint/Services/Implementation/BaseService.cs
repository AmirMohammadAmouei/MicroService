using System.Net;
using System.Text;
using Coupon.EndPoint.Models;
using Coupon.EndPoint.Services.Contracts;
using static Coupon.EndPoint.Utility.SD;
using Newtonsoft.Json;

namespace Coupon.EndPoint.Services.Implementation
{
    public class BaseService : IBaseService
    {
        //Inject for HttpClient to call API => HttpClient in new Dot net core move on HttpClientFactory
        private readonly IHttpClientFactory _httpClientFactory;
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseDto?> SendAsync(RequestDto request)
        {
            try
            {
                //Call Api
                HttpClient client = _httpClientFactory.CreateClient("MangoAPI");

                //Use HttpRequestMessage for provide or configure options
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                //Specify the url that we will invoke to access any API

                message.RequestUri = new Uri(request.Url);                
                //for post and put request we have to serialize the data that we receive

                if (request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8,
                        "application/json");
                }

                HttpResponseMessage? apiResponse = null;

                switch (request.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                //get response back
                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found." };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied." };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "UnAuthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error." };
                    default:
                        //success and convert serialized object to deSerializeObject
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception e)
            {
                var dto = new ResponseDto
                {
                    Message = e.Message.ToString(),
                    IsSuccess = false
                };
                return dto;
            }

        }
    }
}
