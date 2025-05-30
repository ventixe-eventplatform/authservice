using WebApi.Models;

namespace WebApi.Services;

public class AuthService(HttpClient httpClient) : IAuthService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<AuthServiceResultModel> RegisterAsync(RegisterRequestModel requestModel)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7050/api/accounts/register", requestModel);

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
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7050/api/accounts/signin", requestModel);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<AuthServiceResultModel>();
            return new AuthServiceResultModel { Success = content!.Success, Message = content.Message, UserId = content.UserId };
        }

        var error = await response.Content.ReadAsStringAsync();

        return new AuthServiceResultModel { Success = false, Error = error };
    }

    public async Task<AuthServiceResultModel> UserExistsAsync(EmailRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7050/api/accounts/exists", request);

        if (!response.IsSuccessStatusCode)
            return new AuthServiceResultModel { Success = false, Error = await response.Content.ReadAsStringAsync() };

        var result = await response.Content.ReadFromJsonAsync<AuthServiceResultModel>();

        return result!.Data is true
            ? new AuthServiceResultModel { Success = true }
            : new AuthServiceResultModel { Success = false, Error = "User does not exist." };
    }
}
