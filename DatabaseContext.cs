using JwtAuthenticationAndIdentityDemo.DatabaseEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthenticationAndIdentityDemo
{
    public class DatabaseContext:IdentityDbContext<CustomUser>
    {
        public DbSet<UserTask> UserTasks { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //configure one to many with Fluent Api ?

            base.OnModelCreating(builder);
            //configure keys
            builder.Entity<CustomUser>()
                .HasKey(user => user.Id);
            
            //configure relationships, RULE : A user can have many tasks and a Task can only be for one user per time

            builder.Entity<CustomUser>()
                .HasMany(t => t.Tasks)
                .WithOne(u => u.CustomUser);

        }

    }
}
