using Booking.Com_Clone_API.Models.Domain;
using Booking.Com_Clone_API.Models.DTO;
using MailKit.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using NZWalks.API.Controllers;
using NZWalks.API.Data;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Booking.Com_Clone_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly NZWalksDbContext dbContext;
        private readonly ILogger<UserRepository> logger;        
        public UserRepository(NZWalksDbContext dbContext, ILogger<UserRepository> logger,IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.logger = logger;           
            _configuration = configuration;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (await EmailExistAsync(user.Email))
            {
                throw new Exception("Email already exists");
            }

            // Generate verification token
            var token = Guid.NewGuid().ToString();

            // Hash the password
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.ConfirmPassword = BCrypt.Net.BCrypt.HashPassword(user.ConfirmPassword);
            user.Password = user.Password;
            user.ConfirmPassword = user.ConfirmPassword;
            // Set verification token and isVerified status
            user.VerificationToken = token;
            user.IsVerified = false;

            // Add user to database
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            // Send verification email
            await SendVerificationEmailAsync(user.Email, token);

            logger.LogInformation($"Registration successful. Please verify your email.: {JsonSerializer.Serialize(user)}");

            return user;
        }

        public async Task<bool> EmailExistAsync(string email)
        {
            return await dbContext.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> VerifyEmailAsync(string email, string token)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return false;
            }               

            // Compare the verification token with the token stored in the user object
            if (user.VerificationToken == token)
            {
                // Mark the email as verified
                user.IsVerified = true;
                //user.VerificationToken = null; // Remove the token after verification
                await dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }


        ///<summary>
        ///method to send mail notification
        /// </summary>
        public async Task SendVerificationEmailAsync(string email, string token)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            var fromAddress = smtpSettings["FromAddress"];
            var fromName = smtpSettings["FromName"];
            if (string.IsNullOrEmpty(fromAddress))
            {
                throw new ArgumentNullException(nameof(fromAddress), "From address is missing or null in SMTP settings.");
            }

            var emailMessage = new MailMessage
            {
                From = new MailAddress(fromAddress, fromName),
                Subject = "Verify your email",
                //https://localhost:7151/api/User/verify-email?email=danishakhtar539%40gmail.com&token=fc60a64e-c2cb-4824-913d-0764040c1c1d
                //Body = $"Please click the following link to verify your email: https://localhost:7151/api/User/verify-email?email={email}&token={token}",
                IsBodyHtml = true
            };
            emailMessage.To.Add(email);
            // HTML email template
            string htmlBody = @"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Email Template</title>
            </head>
            <body>
                <div>
                    <h1 style=""background-color: #007bff; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px; margin-top:10px; margin-bottom:15px;width:150px;"">Booking.com</h1>
                    <h2>Verify your email address</h2>
                    <p>""Your created an account with the email address:" +email+@""".<br/> Click Confirm to verify the email address and unlock your full account.<br/> We'll also import any bookings you've made with that address""</p>
                    <a href=""https://localhost:7151/api/User/verify-email?email=" + email + "&token=" + token + @""" style=""background-color: #007bff; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px;"">Confirm</a>
                </div>
                
            </body>
            </html>";
            emailMessage.Body = htmlBody;

            using (var client = new SmtpClient(smtpSettings["SmtpServer"], int.Parse(smtpSettings["Port"])))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                client.EnableSsl = true;

                try
                {
                    await client.SendMailAsync(emailMessage);
                    logger.LogInformation($"Verification email sent successfully to {email}.");
                }
                catch (Exception ex)
                {
                    logger.LogError($"Failed to send verification email to {email}. Error: {ex.Message}");
                    throw; // Rethrow the exception to propagate it up the call stack
                }
            }
        }

    }
}
