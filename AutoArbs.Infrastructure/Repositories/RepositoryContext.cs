using AutoArbs.API.Configuration;
using AutoArbs.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AutoArbs.Infrastructure.Repositories
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<Admin>? Admins { get; set; }
        public DbSet<Withdrawal>? WithdrawalHistories { get; set; }
        public DbSet<Deposit>? DepositHistories { get; set; }
        public DbSet<Otp>? Otps { get; set; }
    }
}
