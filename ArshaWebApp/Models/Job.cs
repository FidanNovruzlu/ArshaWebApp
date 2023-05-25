namespace ArshaWebApp.Models;
public class Job
{
    public Job()
    {
        teams= new List<Team>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Team> teams { get; set; }
}
