using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Zip.Installments.DomainEntities;

namespace Zip.Installments.Repositories.AppContext
{
    
    public class ApplicationDbContext : DbContext
    {
        public IConfiguration _configuration { get; }
        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ApplicationDbContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        public virtual DbSet<Installment> tblInstallment { get; set; } 
        public virtual DbSet<PaymentPlan> tblPaymentPlan { get; set; }
     }
}
