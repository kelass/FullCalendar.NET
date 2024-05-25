using Calendar.Data.Configuration;
using Calendar.Models.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Calendar.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<CalendarUser, IdentityRole<string>, string>
    {
        public DbSet<CalendarUser> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<VacancyCategory> VacancyCategories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new VacancyConfiguration());
        }
    }
}
