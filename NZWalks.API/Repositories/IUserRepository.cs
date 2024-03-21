using Booking.Com_Clone_API.Models.Domain;

namespace Booking.Com_Clone_API.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User users);
        Task<bool> EmailExistAsync(string email);
        Task<bool> VerifyEmailAsync(string email, string token);
        Task SendVerificationEmailAsync(string email, string verificationToken);
    }
}
