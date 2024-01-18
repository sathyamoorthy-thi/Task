using Microsoft.EntityFrameworkCore;
using PaymentClaimApi.Models;
using System.ComponentModel.DataAnnotations;


namespace PaymentClaimApi.Data
{
    public class ApplicationDbContext : DbContext
    {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {

            }    
       public DbSet<ClaimDetails> detail { get; set; }

       public virtual DbSet<User> user { get;set; }

    }
}