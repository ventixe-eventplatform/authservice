using System.Net.Http.Headers;
using WebApi.Models;

namespace WebApi.Services;

public class AuthService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor contextAccessor) : IAuthService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public async Task<AuthServiceResultModelT<SignInResponseModel>> RegisterAsync(RegisterRequestModel requestModel)
    {
        var baseUrl = _configuration["AccountServiceBaseUrl"];
        var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/api/accounts/register", requestModel);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<AuthServiceResultModelT<SignInResponseModel>>();
            return content;
        }

        var error = await response.Content.ReadAsStringAsync();

        return new AuthServiceResultModelT<SignInResponseModel> { Success = false, Error = error };
    }

    public async Task<AuthServiceResultModelT<SignInResponseModel>> SignInAsync(SignInRequestModel requestModel)
    {
        var baseUrl = _configuration["AccountServiceBaseUrl"];
        var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/api/accounts/signin", requestModel);

        var raw = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<AuthServiceResultModelT<SignInResponseModel>>();
            return content;
        }

        var error = await response.Content.ReadAsStringAsync();

        return new AuthServiceResultModelT<SignInResponseModel> { Success = false, Error = error };
    }

    public async Task SignOutAsync()
    {
        var baseUrl = _configuration["AccountServiceBaseUrl"];
        var token = _contextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

        var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/api/accounts/signout");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token?.Replace("Bearer ", ""));
        request.Content = null;
        
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            throw new ApplicationException("Signout failed.");
    }

    public async Task<AuthServiceResultModel> UserExistsAsync(EmailRequest request)
    {
        var baseUrl = _configuration["AccountServiceBaseUrl"];
        var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/api/accounts/exists", request);

        if (!response.IsSuccessStatusCode)
            return new AuthServiceResultModel { Success = false, Error = await response.Content.ReadAsStringAsync() };

        var result = await response.Content.ReadFromJsonAsync<AuthServiceResultModel>();

        return result!.Success
            ? new AuthServiceResultModel { Success = true }
            : new AuthServiceResultModel { Success = false, Error = "User does not exist."};
    }
}
