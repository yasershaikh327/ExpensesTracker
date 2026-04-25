using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

public class DbPostgreContext : DbContext
{
    public DbPostgreContext(DbContextOptions<DbPostgreContext> options) : base(options) { }

    public DbSet<Registration> registrations { get; set; }
    public DbSet<LoginLogs> loginLogs { get; set; }
    public DbSet<Contact> contact { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");

    }

}
