using Microsoft.EntityFrameworkCore;
using NotifyMe.Models;

namespace NotifyMe.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<EmailLog> EmailLogs { get; set; }
}
