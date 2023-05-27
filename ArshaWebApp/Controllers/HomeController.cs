using ArshaWebApp.DataContext;
using ArshaWebApp.Models;
using ArshaWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ArshaWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ArshaDbContext _context;
        public HomeController(ArshaDbContext context)
        {
            _context = context;
        }
        public async Task< IActionResult> Index()
        {
            List<Team> teams = await _context.Teams.OrderBy(t=>t.Name).Take(4).Include(t=>t.Job).ToListAsync();
            HomeVM homeVM= new ()
            {
                Teams= teams
            };
            return View(homeVM);
        }
    }
}