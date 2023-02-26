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
        private readonly ILogger<BaseService> _logger;
        public BaseService(IHttpClientFactory httpClient, ILogger<BaseService> logger)
        {
            this.responseModel = new ResponseDTO();
            _httpClient = httpClient;
            _logger = logger;   
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("Api");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                _logger.LogInformation("url1 - " + apiRequest.Url);
                message.RequestUri = new Uri(apiRequest.Url);
                _logger.LogInformation("url2 - " + message.RequestUri.ToString());
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
                _logger.LogInformation("url - " + message.RequestUri.ToString());
                HttpResponseMessage apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                _logger.LogInformation("apiContent - " + apiContent);
                if (apiContent.Contains("Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException"))
                {
                    throw new UnauthorizedAccessException("Невалидный токен");
                }
                if(apiContent.Contains("System.InvalidOperationException"))
                {
                    throw new InvalidOperationException("Ошибка операции");
                }
                var apiResponseDTO = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponseDTO!;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogInformation("UnauthorizedAccessException - " + ex.Message);
                throw new UnauthorizedAccessException("Невалидный токен", ex);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogInformation("InvalidOperationException - " + ex.Message);
                //кастомная ошибка без вывода стека. Если нужен стек - пишем здесь в лог.
                throw new CustomException("Ошибка выполнения операции");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("error - " + ex.Message);
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
