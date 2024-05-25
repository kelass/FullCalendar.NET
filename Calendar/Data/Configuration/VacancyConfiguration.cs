using Calendar.Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Calendar.Data.Configuration
{
    public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
    {
        public void Configure(EntityTypeBuilder<Vacancy> builder)
        {
            builder.HasOne(u => u.VacancyCategory).WithMany().HasForeignKey(a => a.VacancyCategoryId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
