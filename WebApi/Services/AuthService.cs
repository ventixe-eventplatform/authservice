using WebApi.Models;

namespace WebApi.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AuthService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<AuthServiceResultModel> RegisterAsync(RegisterRequestModel requestModel)
    {
        var baseUrl = _configuration["AccountServiceBaseUrl"];
        var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/api/accounts/register", requestModel);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<AuthServiceResultModel>();
            return new AuthServiceResultModel { Success = content!.Success, Message = content.Message, UserId = content.UserId };
        }

        var error = await response.Content.ReadAsStringAsync();

        return new AuthServiceResultModel { Success = false, Error = error };
    }

    public async Task<AuthServiceResultModel> SignInAsync(SignInRequestModel requestModel)
    {
        var baseUrl = _configuration["AccountServiceBaseUrl"];
        var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/api/accounts/signin", requestModel);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<AuthServiceResultModel>();
            return new AuthServiceResultModel { Success = content!.Success, Message = content.Message, UserId = content.UserId };
        }

        var error = await response.Content.ReadAsStringAsync();

        return new AuthServiceResultModel { Success = false, Error = error };
    }

    public async Task SignOutAsync()
    {
        var baseUrl = _configuration["AccountServiceBaseUrl"];
        var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/api/accounts/signout", "");

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

        return result!.Data is true
            ? new AuthServiceResultModel { Success = true }
            : new AuthServiceResultModel { Success = false, Error = "User does not exist." };
    }
}
