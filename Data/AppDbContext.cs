﻿using Microsoft.EntityFrameworkCore;
using SmartAppointments.App.Models;

namespace SmartAppointments.App.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Organization> Organizations => Set<Organization>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Staff> Staff => Set<Staff>();
        public DbSet<Appointment> Appointments => Set<Appointment>();

        // SQL Server connection string -> change Server/User/Password as needed
        // Example for local SQL Server with SQL authentication:
        //   Server=localhost;Database=SmartAppointmentsDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;
        // Example for Windows authentication:
        //   Server=localhost;Database=SmartAppointmentsDb;Trusted_Connection=True;TrustServerCertificate=True;
        private readonly string _cs =
            "Server=localhost;Database=SmartAppointmentsDb;Trusted_Connection=True;TrustServerCertificate=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(_cs);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relationships
            modelBuilder.Entity<Staff>()
                .HasOne(s => s.Organization)
                .WithMany()
                .HasForeignKey(s => s.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Organization)
                .WithMany()
                .HasForeignKey(a => a.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Customer)
                .WithMany()
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Staff)
                .WithMany()
                .HasForeignKey(a => a.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data
            modelBuilder.Entity<Organization>().HasData(
                new Organization { OrganizationId = 1, Name = "City DMV - Downtown", Location = "Main St" },
                new Organization { OrganizationId = 2, Name = "General Hospital", Location = "North Wing" },
                new Organization { OrganizationId = 3, Name = "First National Bank", Location = "Branch A" }
            );

            modelBuilder.Entity<Staff>().HasData(
                new Staff { StaffId = 1, OrganizationId = 1, Name = "Alex Rivera", Role = "Examiner" },
                new Staff { StaffId = 2, OrganizationId = 2, Name = "Dr. Chen", Role = "Nurse" },
                new Staff { StaffId = 3, OrganizationId = 3, Name = "Jamie Patel", Role = "Teller" }
            );
        }
    }
}
