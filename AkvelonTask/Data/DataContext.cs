using AkvelonTask.Models;
using Microsoft.EntityFrameworkCore;

namespace AkvelonTask.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options):base(options)
        {

        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectTask> Tasks { get; set; }
    }
}
