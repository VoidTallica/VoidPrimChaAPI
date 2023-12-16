using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrimaveraAPI.DTO;

namespace PrimaveraAPI.Data
{
    public class AppDBContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public DbSet<SalesItemDTO> SalesItem { get; set; }
        public DbSet<SalesUnitDTO> SalesUnit { get; set; }
        public DbSet<CustomerDTO> Customer { get; set; }
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }
    }
}
