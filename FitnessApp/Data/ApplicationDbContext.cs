using FitnessApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<Person>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Nutrition> Nutritions { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<TrainersMember> TrainersMembers { get; set; }
        public DbSet<GymBundle> GymBundles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            //modelBuilder.Ignore<IdentityUserRole<string>>();
            //modelBuilder.Ignore<IdentityUserClaim<string>>();
            //modelBuilder.Ignore<IdentityUserToken<string>>();
            //modelBuilder.Ignore<IdentityUser<string>>();
            //modelBuilder.Ignore<Person>();
            modelBuilder.Entity<TrainersMember>(entity =>
            {
                // Define the composite primary key using Fluent API
                entity.HasKey(e => new { e.TrainerId, e.MemberId });
            });
        }
    }
}
