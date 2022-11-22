using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleToDoApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleToDoApi.Data
{
    public class ApplicationDbContext :IdentityDbContext
    {
        public virtual DbSet<ToDoItem> items { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public virtual DbSet<User> Users { get; set; }
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries<User>())
            {
                switch (item.State)
                {
                    case EntityState.Modified:
                        item.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Added:
                        item.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                        .Property(f => f.Id)
                        .ValueGeneratedOnAdd();
        }
    

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options)
          :base(options)
        {

        }
    }
}
