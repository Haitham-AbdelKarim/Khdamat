using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Khdamat.Models;

namespace Khdamat.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Khdamat.Models.Account> Account { get; set; }
        public DbSet<Khdamat.Models.Client> Client { get; set; }
        public DbSet<Khdamat.Models.Worker> Worker { get; set; }
    }
}
