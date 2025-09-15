using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.Identity;
using MyAlbum.Application.RolePerm;
using MyAlbum.Models.Identity;
using MyAlbum.Models.ViewModel.Identity;
using MyAlbum.Shared.Enums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAlbum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _iIdentityService;
        private readonly IEmpRolePermissionService _empRolePermissionService;
        public IdentityController(IIdentityService iIdentityService, IEmpRolePermissionService empRolePermissionService)
        {
            _iIdentityService = iIdentityService;
            _empRolePermissionService = empRolePermissionService;
        }

        // GET: /<controller>/
        [HttpGet, AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            LoginReq loginReq = new LoginReq()
            {
                LoginName = model.UserName,
                Password = model.Password
            };

            var loginResult = await _iIdentityService.Login(loginReq);

            if (!loginResult.IsLoginSuccess)
            {
                ModelState.AddModelError(string.Empty, "帳號或密碼錯誤。");
                return View(model);
            }

            List<string> permCodes = new List<string>();
            // 例如 Login 成功後
            var permCodesResp = await _empRolePermissionService.GetEmpRolePermissionCodesAsync(loginResult.EmployeeId);
            if (permCodesResp.StatusCode == (long)ReturnCode.Succeeded && permCodesResp.Data.Count > 0)
                permCodes = permCodesResp.Data;

            // 構造 Claims（重點：帶上 UserType 以配合授權政策）
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, loginResult.AccountId.ToString()),
                new Claim(ClaimTypes.Name, loginResult.FullName),
                new Claim("UserType", loginResult.UserType)
            };
            claims.AddRange(permCodes.Select(p => new Claim("perm", p)));

            // 依 UserType 選擇對應的 Cookie Scheme
            var scheme = "AdminAuth";
            var identity = new ClaimsIdentity(claims, scheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(scheme, principal, new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            });

            // 安全導回
            var fallbackUrl = Url.Action("Index", "Home", new { area = "Admin" });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return Redirect(fallbackUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await HttpContext.SignOutAsync("AdminAuth"); // <- 後台 Cookie scheme
            Response.Cookies.Delete("adminAuth");

            // 回到後台登入頁或指定頁
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Login", "Identity", new { area = "Admin" });
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Denied() => View();
    }
}

