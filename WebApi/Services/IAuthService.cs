using WebApi.Models;

namespace WebApi.Services;

public interface IAuthService
{
    Task<AuthServiceResultModel> RegisterAsync(RegisterRequestModel requestModel);
    Task<AuthServiceResultModel> SignInAsync(SignInRequestModel requestModel);
    Task<AuthServiceResultModel> UserExistsAsync(EmailRequest request);
}
