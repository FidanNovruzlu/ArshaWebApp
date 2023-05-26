using ArshaWebApp.Models;
using ArshaWebApp.ViewModels.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Net;
using System.Net.Mail;

namespace ArshaWebApp.Controllers;
public class AccountController:Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if(!ModelState.IsValid) return View();

        AppUser appUser=new AppUser()
        {
            Name= registerVM.Name,
            Surname= registerVM.Surname,
            Email= registerVM.Email,
            UserName=registerVM.Username
        };

        IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerVM.Password);
        if (!identityResult.Succeeded)
        {
            foreach(IdentityError? error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
                return View();
            }
        }
        
        IdentityResult resultRole = await _userManager.AddToRoleAsync(appUser, "SuperAdmin");
        if (!resultRole.Succeeded)
        {
            foreach (IdentityError? error in resultRole.Errors)
            {
                ModelState.AddModelError("", error.Description);
                return View();
            }
        }

        string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
        string link = Url.Action("ConfrimUser", "Account", new {email=appUser.Email,token=token},HttpContext.Request.Scheme);

        MailMessage message= new MailMessage("fidan.novruzlu0309@gmail.com",appUser.Email)
        {
            Subject="Confrim User",
            Body = $"<a href=\"{link}\">Click to confirm mail</a>",
            IsBodyHtml =true,
        };
        SmtpClient smtpClient = new SmtpClient()
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential("fidan.novruzlu0309@gmail.com", "qxodsjodqajkbibv")
        };    

        smtpClient.Send(message);
        return RedirectToAction(nameof(Login));
    }
    public async Task<IActionResult> ConfrimUser(string email,string token)
    {
        AppUser user= await _userManager.FindByEmailAsync(email);
        if (user == null) return NotFound();

        IdentityResult result= await _userManager.ConfirmEmailAsync(user,token);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Confrim mail incorrect.");
            return View();
        }
         await _signInManager.SignInAsync(user,true);
        return RedirectToAction("Index","Home");
    }
    public async Task<IActionResult> Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if(!ModelState.IsValid) return View();

        AppUser user= await _userManager.FindByEmailAsync(loginVM.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "Invalid password or email!");
            return View();
        }
        Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user,loginVM.Password,true,false);
        if (!signInResult.Succeeded)
        {
            ModelState.AddModelError("", "Invalid password or email.");
            return View();
        }
        return RedirectToAction("Index","Home");
    }

    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    #region AddRole
    //public async Task<IActionResult> AddRole()
    //{
    //    IdentityRole role=new IdentityRole("SuperAdmin");
    //    await _roleManager.CreateAsync(role);
    //    return Json("OK");
    //}
    #endregion
}