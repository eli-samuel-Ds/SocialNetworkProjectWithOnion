using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SocialNetworkProject.Core.Application.Dtos.Account;
using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Core.Application.ViewModels.User;
using SocialNetworkProject.Core.Domain.Common.Enums;
using SocialNetworkProject.Infrastructure.Identity.Entities;
using System.Text;

namespace SocialNetworkProject.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task SetProfilePictureAsync(string userId, string profilePictureUrl)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.ProfilePictureUrl = profilePictureUrl;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No existe una cuenta con el nombre de usuario '{request.UserName}'.";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                response.HasError = true;
                if (!user.EmailConfirmed)
                {
                    response.Error = "Su cuenta no ha sido activada. Por favor, revise su correo electrónico para activar su cuenta.";
                }
                else
                {
                    response.Error = "Usuario o contraseña incorrectos.";
                }
                return response;
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }

        public async Task<AuthenticationResponse> RegisterUserAsync(RegisterRequest request, string origin)
        {
            AuthenticationResponse response = new() { HasError = false };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"El nombre de usuario '{request.UserName}' ya está en uso.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"El correo electrónico '{request.Email}' ya está en uso.";
                return response;
            }

            var user = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.UserNormal.ToString());
                string verificationUri = await GetVerificationEmailUri(user, origin);

                await _emailService.SendAsync(new Core.Application.Dtos.Email.EmailRequestDto
                {
                    To = user.Email,
                    Subject = "Activación de cuenta - SocialNetwork",
                    HtmlBody = $"<p>¡Hola {user.FirstName}!</p><p>Gracias por registrarte. Para activar tu cuenta, por favor haz clic en el siguiente enlace:</p><a href='{verificationUri}'>Activar mi cuenta</a>"
                });

                response.Id = user.Id;
                response.UserName = user.UserName;
                response.Email = user.Email;
                response.IsVerified = user.EmailConfirmed;
                response.Roles = new List<string> { Roles.UserNormal.ToString() };
            }
            else
            {
                response.HasError = true;
                response.Error = string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return response;
        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "Usuario no encontrado.";

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return "¡Tu cuenta ha sido activada exitosamente! Ya puedes iniciar sesión.";
            }
            return "Ocurrió un error al intentar activar tu cuenta.";
        }

        public async Task<AuthenticationResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            AuthenticationResponse response = new() { HasError = false };
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No existe una cuenta con el nombre de usuario '{request.UserName}'.";
                return response;
            }

            user.EmailConfirmed = false;
            await _userManager.UpdateAsync(user);

            string resetUri = await GetResetPasswordUri(user, request.Origin);
            await _emailService.SendAsync(new Core.Application.Dtos.Email.EmailRequestDto
            {
                To = user.Email,
                Subject = "Restablecer contraseña - SocialNetwork",
                HtmlBody = $"<p>Para restablecer tu contraseña, haz clic en el siguiente enlace:</p><a href='{resetUri}'>Restablecer Contraseña</a>"
            });

            return response;
        }

        public async Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            AuthenticationResponse response = new() { HasError = false };
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                response.HasError = true;
                response.Error = "Usuario no encontrado.";
                return response;
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = string.Join(", ", result.Errors.Select(e => e.Description));
                return response;
            }

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<EditProfileViewModel> GetProfileForEditAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            var viewModel = _mapper.Map<EditProfileViewModel>(user);
            return viewModel;
        }

        public async Task<ProfileDto> GetProfileForEditDtoAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ProfileDto { Id = "", FirstName = "", LastName = "", PhoneNumber = "" };
            }

            var profileDto = _mapper.Map<ProfileDto>(user);
            return profileDto;
        }

        public async Task<AuthenticationResponse> UpdateProfileAsync(UpdateProfileRequest request)
        {
            AuthenticationResponse response = new() { HasError = false };
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                response.HasError = true;
                response.Error = "Usuario no encontrado.";
                return response;
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(request.ProfilePictureUrl))
            {
                user.ProfilePictureUrl = request.ProfilePictureUrl;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = string.Join(", ", result.Errors.Select(e => e.Description));
                return response;
            }

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, request.Password);
                if (!passwordResult.Succeeded)
                {
                    response.HasError = true;
                    response.Error = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                    return response;
                }
            }

            return response;
        }

        private async Task<string> GetVerificationEmailUri(AppUser user, string origin)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var route = "User/ConfirmEmail";
            var verificationUri = QueryHelpers.AddQueryString($"{origin}/{route}", "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", token);
            return verificationUri;
        }

        private async Task<string> GetResetPasswordUri(AppUser user, string origin)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var route = "User/ResetPassword";
            var resetUri = QueryHelpers.AddQueryString($"{origin}/{route}", "userId", user.Id);
            resetUri = QueryHelpers.AddQueryString(resetUri, "token", token);
            return resetUri;
        }
    }
}
