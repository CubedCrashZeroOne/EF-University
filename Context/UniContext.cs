using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using EFProject.Context.Entities;

namespace EFProject.Context
{
    class UniContext : DbContext
    {
        private readonly string _connectionString;


        public UniContext() : this(@"data source=.\SQLEXPRESS;initial catalog=UniversityGroups;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework")
        {
        }

        public UniContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Group> Groups { get; set; }
        
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().HasIndex(g => g.Name).IsUnique();

            modelBuilder.Entity<Group>().HasData(
                new Group { Id = 1, Name = "Радиофизика" },
                new Group { Id = 2, Name = "Микроэлектроника"},
                new Group { Id = 3, Name = "Общая физика" }
                );
        }
    }
}
