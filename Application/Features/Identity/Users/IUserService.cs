namespace Application.Features.Identity.Users;

public interface IUserService
{
    Task<string> CreateUserAsync(CreateUserRequest request);
    Task<string> UpdateUserAsync(UpdateUserRequest request);
    Task<string> DeleteUserAsync(string id);
    Task<string> ActivateOrDeactivateUserAsync(string userId, bool activation);
    Task<string> ChangePasswordAsync(ChangePasswordRequest request);
    Task<string> AssignRolesAsync(string userId, UserRoleRequest request);
}
