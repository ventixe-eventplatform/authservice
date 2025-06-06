using WebApi.Models;

namespace WebApi.Services;

public interface IAuthService
{
    Task<AuthServiceResultModelT<SignInResponseModel>> RegisterAsync(RegisterRequestModel requestModel);
    Task<AuthServiceResultModelT<SignInResponseModel>> SignInAsync(SignInRequestModel requestModel);
    Task SignOutAsync();
    Task<AuthServiceResultModel> UserExistsAsync(EmailRequest request);
}
