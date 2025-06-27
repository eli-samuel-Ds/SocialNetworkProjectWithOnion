using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkProject.Core.Application.Dtos.Account;
using SocialNetworkProject.Core.Application.Interfaces;
using SocialNetworkProject.Core.Application.ViewModels.User;
using SocialNetworkProject.Helpers;
using SocialNetworkProject.Middlewares;

namespace SocialNetworkProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IFileUploader _fileUploader; 
        private readonly IMapper _mapper;

        public UserController(IAccountService accountService, IMapper mapper, IFileUploader fileUploader) 
        {
            _accountService = accountService;
            _mapper = mapper;
            _fileUploader = fileUploader; 
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {
            return View(new LoginViewModel()
            {
                Password = string.Empty,
                UserName = string.Empty
            });
        }

        [HttpPost]
        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AuthenticationRequest request = new() { UserName = vm.UserName, Password = vm.Password };
            var response = await _accountService.AuthenticateAsync(request);

            if (response.HasError)
            {
                vm.Password = "";
                ModelState.AddModelError("LoginError", response.Error);
                return View(vm);
            }

            HttpContext.Session.Set("user", response);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToAction("Index");
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Register()
        {
            return View(new RegisterViewModel()
            {
                ConfirmPassword = string.Empty,
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                PhoneNumber = string.Empty,
                Password = string.Empty,
                ProfilePictureFile = null,
                UserName = string.Empty
            });
        }

        [HttpPost]
        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            string origin = Request.Scheme + "://" + Request.Host;
            var request = _mapper.Map<RegisterRequest>(vm);

            var response = await _accountService.RegisterUserAsync(request, origin);
            if (response.HasError)
            {
                vm.Password = "";
                vm.ConfirmPassword = "";
                ModelState.AddModelError("RegisterError", response.Error);
                return View(vm);
            }

            if (!response.HasError && vm.ProfilePictureFile != null)
            {
                string imageUrl = _fileUploader.UploadFile(vm.ProfilePictureFile, response.Id, "profiles");
                await _accountService.SetProfilePictureAsync(response.Id, imageUrl);
            }

            ViewBag.SuccessMessage = "¡Registro exitoso! Por favor, revisa tu correo para activar tu cuenta.";
            return View("Index", new LoginViewModel()
            {
                Password = string.Empty,
                UserName = string.Empty
            });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string response = await _accountService.ConfirmEmailAsync(userId, token);
            ViewBag.ConfirmationMessage = response;
            return View("Index", new LoginViewModel()
            {
                Password = string.Empty,
                UserName = string.Empty
            });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel()
            {
                UserName = string.Empty
            });
        }

        [HttpPost]
        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            string origin = Request.Scheme + "://" + Request.Host;
            ForgotPasswordRequest request = new() { UserName = vm.UserName, Origin = origin };
            var response = await _accountService.ForgotPasswordAsync(request);

            if (response.HasError)
            {
                ModelState.AddModelError("ForgotPasswordError", response.Error);
                return View(vm);
            }

            ViewBag.SuccessMessage = "Se ha enviado un correo con las instrucciones para restablecer tu contraseña.";
            return View("Index", new LoginViewModel()
            {
                Password = string.Empty,
                UserName = string.Empty
            });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult ResetPassword(string userId, string token)
        {
            return View(new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token,
                ConfirmPassword = string.Empty,
                Password = string.Empty
            });
        }

        [HttpPost]
        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            ResetPasswordRequest request = new() { UserId = vm.UserId, Token = vm.Token, Password = vm.Password };
            var response = await _accountService.ResetPasswordAsync(request);

            if (response.HasError)
            {
                ModelState.AddModelError("ResetPasswordError", response.Error);
                return View(vm);
            }

            ViewBag.SuccessMessage = "¡Tu contraseña ha sido restablecida exitosamente! Ya puedes iniciar sesión.";
            return View("Index", new LoginViewModel()
            {
                Password = string.Empty,
                UserName = string.Empty
            });
        }


        [ServiceFilter(typeof(UserAuthorize))]
        public async Task<IActionResult> Profile()
        {
            var userSession = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (userSession?.Id == null)
            {
                return RedirectToAction("Index");
            }

            var profileDto = await _accountService.GetProfileForEditDtoAsync(userSession.Id);

            var vm = _mapper.Map<EditProfileViewModel>(profileDto);

            return View(vm);
        }

        [HttpPost]
        [ServiceFilter(typeof(UserAuthorize))]
        public async Task<IActionResult> Profile(EditProfileViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var userSession = HttpContext.Session.Get<AuthenticationResponse>("user");
            vm.Id = userSession.Id;

            if (!string.IsNullOrWhiteSpace(vm.Password))
            {
                if (string.IsNullOrWhiteSpace(vm.ConfirmPassword))
                {
                    ModelState.AddModelError("ConfirmPassword", "Debe confirmar la contraseña.");
                    return View(vm);
                }
                if (vm.Password != vm.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden.");
                    return View(vm);
                }
            }

            var request = _mapper.Map<UpdateProfileRequest>(vm);

            if (vm.ProfilePictureFile != null)
            {
                request.ProfilePictureUrl = _fileUploader.UploadFile(vm.ProfilePictureFile, userSession.Id, "profiles", true, vm.ProfilePictureUrl);
            }

            var response = await _accountService.UpdateProfileAsync(request);

            if (response.HasError)
            {
                ModelState.AddModelError("ProfileError", response.Error);
                return View(vm);
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}
