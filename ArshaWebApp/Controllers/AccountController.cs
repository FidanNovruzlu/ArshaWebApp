using ArshaWebApp.Models;
using ArshaWebApp.ViewModels.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

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

        return RedirectToAction(nameof(Login));
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