using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleToDoApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleToDoApi.Data
{
    public class ApplicationDbContext :IdentityDbContext
    {
        public virtual DbSet<ToDoItem> items { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options)
          :base(options)
        {

        }
    }
}
