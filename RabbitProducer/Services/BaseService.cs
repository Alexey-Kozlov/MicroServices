using Models;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;
using System.Net.Http.Headers;
using Common;

namespace Services
{
    public class BaseService : IBaseServise
    {
        public ResponseDTO responseModel { get; set; }
        private readonly IHttpClientFactory _httpClient;
        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new ResponseDTO();
            _httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("Api");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();

                if (apiRequest.Data != null)
                {                    
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, MediaTypeNames.Application.Json);
                }

                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }

                switch (apiRequest.ApiType)
                {
                    case ApiType.Post:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.Get:
                        message.Method = HttpMethod.Get;
                        break;
                    case ApiType.Delete:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                HttpResponseMessage apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                if (apiContent.Contains("Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException"))
                {
                    throw new UnauthorizedAccessException("Невалидный токен");
                }
                if (apiContent.Contains("System.InvalidOperationException"))
                {
                    throw new InvalidOperationException("Ошибка операции");
                }
                var apiResponseDTO = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponseDTO!;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("Невалидный токен", ex);
            }
            catch (InvalidOperationException)
            {
                //кастомная ошибка без вывода стека. Если нужен стек - пишем здесь в лог.
                throw new CustomException("Ошибка выполнения операции");
            }
            catch (Exception ex)
            {
                var dtoError = new ResponseDTO
                {
                    Message = "Ошибка",
                    Errors = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                var result = JsonConvert.SerializeObject(dtoError, Formatting.Indented);
                var apiResponseDTO = JsonConvert.DeserializeObject<T>(result);
                return apiResponseDTO!;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }


    }
}
