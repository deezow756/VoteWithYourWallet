using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoteWithYourWallet.Models;

namespace VoteWithYourWallet.Database
{
    public class AuthDbContext: IdentityDbContext
    {
        public DbSet<Cause> Causes { get; set; }
        public DbSet<Signature> Signatures { get; set; } 

        public AuthDbContext(DbContextOptions<AuthDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Signature>().HasKey(a => new { a.IdentityUserId, a.CauseId });
        }
    }
}
