using ArshaWebApp.DataContext;
using ArshaWebApp.Extensions;
using ArshaWebApp.Models;
using ArshaWebApp.ViewModels;
using ArshaWebApp.ViewModels.TeamVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArshaWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles ="SuperAdmin")]
public class TeamController : Controller
{
    private readonly ArshaDbContext _arshaDbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public TeamController(ArshaDbContext arshaDbContext, IWebHostEnvironment webHostEnvironment)
    {
        _arshaDbContext = arshaDbContext;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index(int page = 1,int take=5)
    {
        List<Team> teams = await _arshaDbContext.Teams.Skip((page-1)*take).Take(take).ToListAsync();
        int teamAllCount = _arshaDbContext.Teams.Count();
        PaginationVM<Team> paginationVM = new()
        {
            Teams= teams,
            CurrentPage=page,
            PageCount= (int) Math.Ceiling((double)teamAllCount / take)
        };
        return View(paginationVM);
    }


    public async Task<IActionResult> Create()
    {
        CreateTeamVM newTeam = new()
        {
            Jobs = await _arshaDbContext.Jobs.ToListAsync(),
        };
        return View(newTeam);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTeamVM newTeam)
    {
        if (!ModelState.IsValid)
        {
            newTeam.Jobs=  await _arshaDbContext.Jobs.ToListAsync();
            return View(newTeam);
        }

        if(!newTeam.ProfileImage.CheckType("image/") && !newTeam.ProfileImage.CheckSize(2048))
        {
            ModelState.AddModelError("", "Incorrect image type and size!");
            newTeam.Jobs=  await _arshaDbContext.Jobs.ToListAsync();
            return View(newTeam);
        }
 
        string newFileName = await newTeam.ProfileImage.UploadAsync(_webHostEnvironment.WebRootPath, "assets", "img", "team");

        Team team = new()
        {
            Name = newTeam.Name,
            Surname = newTeam.Surname,
            Bio = newTeam.Bio,
            JobId = newTeam.JobId,
            ProfileImageName = newFileName
        };

        await _arshaDbContext.Teams.AddAsync(team);
        await _arshaDbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        Team? team = await _arshaDbContext.Teams.FindAsync(id);
        if (team == null) return NotFound();

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team", team.ProfileImageName);
        if(System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
        _arshaDbContext.Teams.Remove(team);
        await _arshaDbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Update(int id)
    {
        Team? team = await _arshaDbContext.Teams.FindAsync(id);
        if (team == null) return NotFound();

        UpdateTeamVM updateTeamVM = new UpdateTeamVM()
        {
            Jobs = await _arshaDbContext.Jobs.ToListAsync(),
            Bio=team.Bio,
            Name=team.Name,
            Surname=team.Surname,
            JobId=team.JobId,
            ProfileImageName=team.ProfileImageName
        };
        return View(updateTeamVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, UpdateTeamVM updateTeamVM)
    {
        Team? team = await _arshaDbContext.Teams.FindAsync(id);
        if (team == null) return NotFound();

        if (!ModelState.IsValid)
        {
            updateTeamVM.Jobs = await _arshaDbContext.Jobs.ToListAsync();
            updateTeamVM.ProfileImageName=team.ProfileImageName;
            return View(updateTeamVM);
        }
        if (updateTeamVM.ProfileImage != null)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team", team.ProfileImageName);
            using(FileStream fileStream=new FileStream(path,FileMode.Create))
            {
                await updateTeamVM.ProfileImage.CopyToAsync(fileStream);
            }
        }
        team.Surname = updateTeamVM.Surname;
        team.Name = updateTeamVM.Name;
        team.Bio = updateTeamVM.Bio;
        team.JobId = updateTeamVM.JobId;

        await _arshaDbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Read(int id)
    {
        Team? team = await _arshaDbContext.Teams.Include(t=>t.Job).FirstOrDefaultAsync(t=>t.Id==id);
        if (team == null) return NotFound();
        return View(team);
;    }
}