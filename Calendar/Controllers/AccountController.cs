using Calendar.Models.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Calendar.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<CalendarUser> _signInManager;
        private readonly UserManager<CalendarUser> _userManager;
        public AccountController(SignInManager<CalendarUser> signInManager, UserManager<CalendarUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet("/account/external-login")]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                new { ReturnUrl = returnUrl });
            var properties =
                _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl!);

            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl ??= Url.Content("/home/index");
            IActionResult actionResult = LocalRedirect(returnUrl);
            var result = await ExternalLoginCallbackAsync(returnUrl, remoteError);
            if (!result.Succeeded)
            {
                ModelState
                    .AddModelError(string.Empty, result.ErrorMessage!);
                actionResult = Redirect("/identity/account/login");
            }

            return actionResult;
        }

        #region private
        private async Task<ExternalLoginResult> ExternalLoginCallbackAsync(string? returnUrl = null, string? remoteError = null)
        {
            ExternalLoginResult result = new();
            if (remoteError != null)
            {
                result.ErrorMessage = $"Error from external provider: {remoteError}";
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                result.ErrorMessage = "Error loading external login information.";
            }

            if (!result.Succeeded)
            {
                var signInResult = await _signInManager.ExternalLoginSignInAsync(info!.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

                if (signInResult.Succeeded)
                {
                    result.Succeeded = true;
                }
                else
                {
                    string email = info.Principal.FindFirstValue(ClaimTypes.Email);
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        bool loginSucceeded = await TryAddExternalLoginAsync(info);
                        if (!loginSucceeded)
                        {
                            CalendarUser user = new()
                            {
                                Id = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier),
                                UserName = email,
                                Email = email,
                                ConcurrencyStamp = Guid.NewGuid().ToString(),
                                SecurityStamp = Guid.NewGuid().ToString(),
                            };
                            var createdResult = await CreateAndExternalLoginAsync(user, info);
                            result.Succeeded = createdResult.Succeeded;
                        }
                        else
                        {
                            result.Succeeded = true;
                        }
                    }
                }
            }
            return result;
        }

        private class ExternalLoginResult
        {
            public string? ErrorMessage { get; set; }
            public bool Succeeded { get; set; }
        }

        private async Task<SignUpResult> CreateAndExternalLoginAsync(CalendarUser user, ExternalLoginInfo info)
        {
            IdentityResult result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            return new SignUpResult { Succeeded = result.Succeeded, UserId = user.Id };
        }

        private class SignUpResult
        {
            public string UserId { get; init; }
            public bool Succeeded { get; init; }
        }

        private async Task<bool> TryAddExternalLoginAsync(ExternalLoginInfo info)
        {
            bool result = false;
            string email = info.Principal.FindFirstValue(ClaimTypes.Email);
            CalendarUser? user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                result = true;
            }
            return result;
        }
        #endregion
    }
}
