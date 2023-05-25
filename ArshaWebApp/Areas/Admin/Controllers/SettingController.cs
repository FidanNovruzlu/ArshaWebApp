using ArshaWebApp.DataContext;
using ArshaWebApp.Models;
using ArshaWebApp.ViewModels.SettingVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Drawing.Printing;

namespace ArshaWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "SuperAdmin")]
public class SettingController : Controller
{
    private readonly ArshaDbContext _arshaDbContext;
    public SettingController(ArshaDbContext arshaDbContext)
    {
        _arshaDbContext = arshaDbContext;
    }
    public IActionResult Index()
    {
        List<Setting> settings=_arshaDbContext.Settings.ToList();
        return View(settings);
    }
    public IActionResult Update(int id)
    {
        Setting? setting= _arshaDbContext.Settings.FirstOrDefault(x => x.Id == id);
        if (setting == null) return NotFound();
        UpdateSettingVM updateSettingVM= new UpdateSettingVM()
        {
            Value= setting.Value,
        };
        return View(updateSettingVM);
    }
    [HttpPost]
    public IActionResult Update(int id, UpdateSettingVM updateSettingVM)
    {
        if (!ModelState.IsValid)
        {
            return View(updateSettingVM);
        }

        Setting? setting = _arshaDbContext.Settings.FirstOrDefault(x => x.Id == id);
        if (setting == null) return NotFound();

        setting.Value = updateSettingVM.Value;

        _arshaDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
