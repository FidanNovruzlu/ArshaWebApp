using ArshaWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArshaWebApp.DataContext;
public class ArshaDbContext:IdentityDbContext<AppUser>
{
	public ArshaDbContext(DbContextOptions<ArshaDbContext> options):base(options)
	{

	}
	public DbSet<Team> Teams { get; set; }
	public DbSet<Job> Jobs { get; set; }
	public DbSet<Setting> Settings { get; set; }	
}
