using ArshaWebApp.DataContext;
using ArshaWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArshaWebApp.ViewComponents;

public class FooterViewComponent:ViewComponent
{
    private readonly ArshaDbContext _context;
	public FooterViewComponent(ArshaDbContext arshaDbContext)
	{
		_context= arshaDbContext;
	}
	public async Task<IViewComponentResult> InvokeAsync()
	{
		Dictionary<string,Setting> setting = await _context.Settings.ToDictionaryAsync(k=>k.Key);
		return View( setting);
	}
}
