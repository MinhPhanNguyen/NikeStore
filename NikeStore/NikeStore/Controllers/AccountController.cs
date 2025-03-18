using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;
using NikeStore.Models.ViewModels;
using NikeStore.Repository;

namespace NikeStore.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManager;
        private SignInManager<AppUserModel> _signInManager;
        private readonly DataContext _datacontext;
        private readonly IEmailSender _emailService;

        public AccountController(IEmailSender emailService, DataContext dataContext,UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager)
        {
            _emailService = emailService;
            _datacontext = dataContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userById = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (userById == null)
            {
                return NotFound();
            }

            return View(userById); 
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePass(string oldPassword, string newPassword)
        {
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                TempData["error"] = "Mật khẩu mới không được để trống.";
                return RedirectToAction("Index", "Account");
            }

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (result.Succeeded)
            {
                TempData["success"] = "Đổi mật khẩu thành công!";
                return RedirectToAction("Index", "Account");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    TempData["error"] = error.Description;
                }
                return RedirectToAction("Index", "Account");
            }
        }

        public IActionResult LogIn(string returnUrl)
        {
            return View(new LogInViewModel { ReturnUrl = returnUrl });
        }

        public async Task<IActionResult> NewPassword(AppUserModel user,string token)
        {
            var checkUser = await _userManager.Users
                .Where(u => u.Token == user.Token)
                .Where(u => u.Email == user.Email)
                .FirstOrDefaultAsync();

            if (checkUser != null)
            {
                ViewBag.Email = checkUser.Email;
                ViewBag.Token = token;
                return View();
            }
            else
            {
                TempData["error"] = "Token không hợp lệ";
                return RedirectToAction("ForgetPass", "Account");
            }
        }

        public async Task<IActionResult> UpdateNewPassword(AppUserModel user, string token)
        {
            Console.WriteLine($"Received token: {token}");

            var checkUser = await _userManager.Users
                .Where(u => u.Token == token)
                .Where(u => u.Email == user.Email)
                .FirstOrDefaultAsync();

            if (checkUser != null)
            {
                Console.WriteLine($"User found: {checkUser.Email}");
                string newToken = Guid.NewGuid().ToString();
                var PasswordHasher = new PasswordHasher<AppUserModel>();
                var PasswordHash = PasswordHasher.HashPassword(checkUser, user.PasswordHash);

                checkUser.PasswordHash = PasswordHash;
                checkUser.Token = newToken; // Cập nhật token mới

                await _userManager.UpdateAsync(checkUser);
                TempData["success"] = "Đổi mật khẩu thành công";
                return RedirectToAction("LogIn", "Account");
            }
            else
            {
                Console.WriteLine("Token không hợp lệ!");
                TempData["error"] = "Token không hợp lệ, vui lòng gửi lại mã";
                return RedirectToAction("ForgetPass", "Account");
            }
        }

        public async Task<IActionResult> ForgetPass()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMailForgotPass(AppUserModel user)
        {
            var checkMail = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (checkMail == null)
            {
                TempData["error"] = "Email không tồn tại";
                return RedirectToAction("ForgetPass", "Account");
            }
            else
            {
                string token = Guid.NewGuid().ToString();
                checkMail.Token = token;
                _datacontext.Update(checkMail);
                await _datacontext.SaveChangesAsync();
                var receiver = checkMail.Email;
                var subject = "Reset Password" + checkMail.Email;
                var message = $"Click here to reset your password: <a href='{Request.Scheme}://{Request.Host}/Account/NewPassword?email={checkMail.Email}&token={token}'>Reset Password</a>";

                await _emailService.SendEmailAsync(receiver, subject, message);
            }

            TempData["success"] = "Vui lòng kiểm tra email để reset mật khẩu";
            return RedirectToAction("ForgetPass", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel login)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(login.AccountName, login.Password, false, false);
                if (result.Succeeded)
                {
                    return Redirect(login.ReturnUrl ?? "/");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(login);
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(Account user)
        {
            if (ModelState.IsValid)
            {
                AppUserModel newUser = new AppUserModel
                {
                    UserName = user.AccountName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                };

                IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    return RedirectToAction("LogIn", "Account");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(user);
        }

        public async Task<IActionResult> LogOut(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
        
        public async Task LogInByGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext
                .AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if(!result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value; 
            string emailName = email.Split('@')[0];
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var newUser = new AppUserModel
                {
                    UserName = emailName,
                    Email = email
                };

                var PasswordHasher = new PasswordHasher<AppUserModel>();
                newUser.PasswordHash = PasswordHasher.HashPassword(newUser, "0918195minh");

                var resultCreate = await _userManager.CreateAsync(newUser);

                if (!resultCreate.Succeeded)
                {
                    TempData["error"] = "Đăng nhập thất bại";
                    return RedirectToAction("LogIn", "Account");
                }
                else
                {
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    TempData["success"] = "Đăng nhập thành công";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
