using Microsoft.EntityFrameworkCore;
using TechAcadStudentsMVC.Models;

namespace TechAcadStudentsMVC.Data
{
    public class InsuranceContext : DbContext
    {
        public InsuranceContext(DbContextOptions<InsuranceContext> options)
            : base(options)
        {
        }

        public DbSet<Insuree> Insurees { get; set; } = default!;
    }
}
