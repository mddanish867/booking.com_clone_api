using Booking.Com_Clone_API.Models.Domain;
using Booking.Com_Clone_API.Models.DTO;

namespace Booking.Com_Clone_API.Repositories
{
    public interface IUserRepository
    {
        // Registration
        ///<summary>
        ///method to create new registration
        /// </summary>
        Task<User> CreateUserAsync(User users);

        ///<summary>
        ///method to check if user already exists
        /// </summary>
        Task<bool> EmailExistAsync(string email);

        
        ///<summary>
        ///method to send mail for verifying user
        /// </summary>
        Task SendVerificationEmailAsync(string email, string verificationToken);

        ///<summary>
        ///method to verify mail
        /// </summary>
        Task<bool> VerifyEmailAsync(string email, string token);

        // Login
        ///<summary>
        ///method to generate JWT token
        /// </summary>
        string GenerateJwtToken(User user);

        ///<summary>
        ///method to generate JWT token
        /// </summary>
        Task<string> GenerateResetPasswordToken(User user);

        ///<summary>
        ///method to send mail for verifying user
        /// </summary>
        Task SendPasswordResetEmailAsync(string email, string token);

        ///<summary>
        ///method to add user address
        /// </summary>
        /// 
        Task<Guid> AddUserAddressAsync(UserAddressDto userDto);

        ///<summary>
        ///method to get user address based on id
        /// </summary>
        Task<UserAddress> GetLatestUserAddressAsync(Guid userId);
    }
}
