#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using RazorCrudAppAuth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace RazorCrudAppAuth.Areas.Identity.Pages.Account
{

    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public class InputModel
        {
            [Required(ErrorMessage = "Поле Email обязательно для заполнения.")]
            [EmailAddress(ErrorMessage = "Некорректный формат Email.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Поле Имя обязательно для заполнения.")]
            [StringLength(100, ErrorMessage = "Имя должно содержать от {2} до {1} символов.", MinimumLength = 2)]
            [Display(Name = "Имя и Фамилия")]
            public string FullName { get; set; }

            [Required(ErrorMessage = "Поле Телефон обязательно для заполнения.")]
            [Phone(ErrorMessage = "Некорректный формат телефона.")]
            [Display(Name = "Телефон")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Поле Пароль обязательно для заполнения.")]
            [StringLength(100, ErrorMessage = "Пароль должен быть длиной от {2} до {1} символов.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Подтверждение пароля")]
            [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
            public string ConfirmPassword { get; set; }
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

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.FullName = Input.FullName;
                user.Role = "User";
                user.PhoneNumber = Input.Phone;
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Пользователь создал аккаунт с паролем.");

                    //if (!await _roleManager.RoleExistsAsync("Admin"))
                    //{
                    //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    //}

                    //await _userManager.AddToRoleAsync(user, "Admin");
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Подтвердите ваш email",
                        $"Пожалуйста, подтвердите аккаунт, перейдя по ссылке: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>подтвердить</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    var errorMsg = TranslateIdentityError(error);
                    ModelState.AddModelError(string.Empty, errorMsg);
                }
                return Page();

            }

            return Page();
        }
        private string TranslateIdentityError(IdentityError error)
        {
            return error.Code switch
            {
                "DuplicateUserName" => "Пользователь с таким Email уже зарегистрирован.", // <- добавь сюда
                "DuplicateEmail" => "Пользователь с таким Email уже зарегистрирован.",
                "PasswordTooShort" => "Пароль слишком короткий.",
                "PasswordRequiresNonAlphanumeric" => "Пароль должен содержать хотя бы один специальный символ.",
                "PasswordRequiresDigit" => "Пароль должен содержать хотя бы одну цифру.",
                "PasswordRequiresLower" => "Пароль должен содержать хотя бы одну строчную букву.",
                "PasswordRequiresUpper" => "Пароль должен содержать хотя бы одну заглавную букву.",
                _ => error.Description
            };
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Невозможно создать экземпляр '{nameof(ApplicationUser)}'. Убедитесь, что класс не абстрактный и имеет конструктор без параметров.");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("Для работы требуется реализация UserStore с поддержкой Email.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
