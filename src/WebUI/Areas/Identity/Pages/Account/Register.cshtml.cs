using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Common;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IDateTime _date;

        public RegisterModel(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<RegisterModel> logger, IEmailSender emailSender, IDateTime date)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(50)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(50)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [StringLength(50)]
            [Display(Name = "User Name")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [StringLength(256)]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 12)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Are you an Adult")]
            public bool IsAdult { get; set; }

            [Required]
            public string Recaptcha { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!await IsReCaptchValid())
            {
                ModelState.AddModelError("Recaptcha", "You must complete the reCaptcha to continue");
                return Page();
            }

            if (ModelState.IsValid)
                return Page();

            var user = new AppUser
            {
                UserName = Input.Username,
                Email = Input.Email,
                CreatedOn = _date.Now,
                FirstName = Input.FirstName,
                IsAdult = Input.IsAdult,
                LastName = Input.LastName
            };
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        public async Task<bool> IsReCaptchValid()
        {
            var result = false;
            var captchaResponse = Request.Form["g-recaptcha-response"];
            if (string.IsNullOrEmpty(captchaResponse))
                return false;
            var secretKey = "6LeMYtcUAAAAAE5KNMRyuHeL_6-rD0jiQuSlL2PR";
            const string apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (var response = await request.GetResponseAsync())
            {
                using var stream = new StreamReader(response.GetResponseStream());
                var jResponse = JObject.Parse(await stream.ReadToEndAsync());
                result = jResponse.Value<bool>("success");
            }
            return result;
        }
    }
}