using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    private string ADMIN_ROLE = "Administrator";
    private string[] adminUsers = { "admin@test.com" };

    public DbSet<UserSetting> UserSettings { get; set; }
    public DbSet<Cryptocoin> Cryptocoins { get; set; }
    public DbSet<Exchange> Exchanges { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
      var entries = ChangeTracker.Entries().Where(E => E.State == EntityState.Added || E.State == EntityState.Modified).ToList();

      foreach (var entry in entries)
      {
        if (entry.State == EntityState.Modified && entry.GetType().GetProperty("UpdatedAt") != null)
        {
          entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
        }
      }

      return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<IdentityRole>(t => t.ToTable("Roles"));
      builder.Entity<ApplicationUser>(t => t.ToTable("Users"));
      builder.Entity<IdentityRoleClaim<string>>(t => t.ToTable("RoleClaims"));
      builder.Entity<IdentityUserLogin<string>>(t => t.ToTable("UserLogins"));
      builder.Entity<IdentityUserRole<string>>(t => t.ToTable("UserRoles"));
      builder.Entity<IdentityUserToken<string>>(t => t.ToTable("UserTokens"));
      builder.Entity<IdentityUserClaim<string>>(t => t.ToTable("UserClaims"));

      this.SeedData(builder);
    }

    private void SeedData(ModelBuilder builder)
    {
      // Roles
      IdentityRole adminRole = new IdentityRole
      {
        Id = Guid.NewGuid().ToString(),
        Name = ADMIN_ROLE,
        NormalizedName = ADMIN_ROLE.ToUpper(),
        ConcurrencyStamp = Guid.NewGuid().ToString(),
      };
      builder.Entity<IdentityRole>().HasData(adminRole);

      // Super User
      var hasher = new PasswordHasher<ApplicationUser>();
      var appUsers = adminUsers.Select(u => new ApplicationUser
      {
        Email = u,
        NormalizedEmail = u.ToUpper(),
        NormalizedUserName = u.ToUpper(),
        UserName = u,
        Name = u,
        EmailConfirmed = true,
        Id = Guid.NewGuid().ToString(),
        SecurityStamp = Guid.NewGuid().ToString(),
        PasswordHash = hasher.HashPassword(null, u)
      }).ToArray();
      builder.Entity<ApplicationUser>().HasData(appUsers);

      // UserRoles
      var roleToUser = appUsers.Select(u => new IdentityUserRole<string> { RoleId = adminRole.Id, UserId = u.Id });
      builder.Entity<IdentityUserRole<string>>().HasData(roleToUser);
    }
  }
}
