using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;

namespace SafeguardSystem.DAL;

public class SafeguardDbContext : DbContext
{
    public SafeguardDbContext(DbContextOptions<SafeguardDbContext> options)
        : base(options)
    {
    }
    public DbSet<Business> Businesses { get; set; }

    public DbSet<Checkpoint> Checkpoints { get; set; }

    public DbSet<Contract> Contracts { get; set; }

    public DbSet<Location> Locations { get; set; }

    public DbSet<RefreshToken> Refreshtokens { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<SecurityGuard> SecurityGuards { get; set; }

    public DbSet<SecurityShift> SecurityShifts { get; set; }

    public DbSet<ShiftAssignment> ShiftAssignments { get; set; }

    public DbSet<ShiftIncident> ShiftIncidents { get; set; }

    public DbSet<ShiftType> ShiftTypes { get; set; }

    public DbSet<Team> Teams { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Seed();

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.BusinessId);
            entity.Property(e => e.Name).IsRequired();
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Businesses)
                  .HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<Checkpoint>(entity =>
        {
            entity.HasKey(e => e.CheckpointId);
            entity.Property(e => e.Name).IsRequired();
            entity.HasOne(e => e.Location)
                  .WithMany(l => l.Checkpoints)
                  .HasForeignKey(e => e.LocationId);
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId);
            entity.Property(e => e.ContractCode).IsRequired();
            entity.HasOne(e => e.Team)
                  .WithMany(t => t.Contracts)
                  .HasForeignKey(e => e.TeamId);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId);
            entity.Property(e => e.Name).IsRequired();
            entity.HasOne(e => e.Business)
                  .WithMany(b => b.Locations)
                  .HasForeignKey(e => e.BusinessId);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenId);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.RefreshTokens)
                  .HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId);
            entity.Property(e => e.RoleName).IsRequired();
        });

        modelBuilder.Entity<SecurityGuard>(entity =>
        {
            entity.HasKey(e => e.GuardId);
            entity.Property(e => e.IdentityNumber).IsRequired();
            entity.HasOne(e => e.User)
                  .WithMany(u => u.SecurityGuards)
                  .HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Team)
                  .WithMany(t => t.Guards)
                  .HasForeignKey(e => e.TeamId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SecurityShift>(entity =>
        {
            entity.HasKey(e => e.ShiftId);
            entity.HasOne(e => e.Location)
                  .WithMany(l => l.SecurityShifts)
                  .HasForeignKey(e => e.LocationId);
            entity.HasOne(e => e.Team)
                  .WithMany(t => t.SecurityShifts)
                  .HasForeignKey(e => e.TeamId);
            entity.HasOne(e => e.Type)
                  .WithMany(st => st.SecurityShifts)
                  .HasForeignKey(e => e.TypeId);
        });

        modelBuilder.Entity<ShiftAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId);
            entity.HasOne(e => e.Shift)
                  .WithMany(s => s.ShiftAssignments)
                  .HasForeignKey(e => e.ShiftId);
            entity.HasOne(e => e.Guard)
                  .WithMany(g => g.ShiftAssignments)
                  .HasForeignKey(e => e.GuardId);
            entity.HasOne(e => e.Location)
                  .WithMany(l => l.ShiftAssignments)
                  .HasForeignKey(e => e.LocationId);
            entity.HasOne(e => e.Checkpoint)
                  .WithMany(c => c.ShiftAssignments)
                  .HasForeignKey(e => e.CheckpointId);
        });

        modelBuilder.Entity<ShiftIncident>(entity =>
        {
            entity.HasKey(e => e.IncidentId);
            entity.HasOne(e => e.Assignment)
                  .WithMany(sa => sa.ShiftIncidents)
                  .HasForeignKey(e => e.AssignmentId);
        });

        modelBuilder.Entity<ShiftType>(entity =>
        {
            entity.HasKey(e => e.TypeId);
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.HasOne(e => e.Role)
                  .WithMany(r => r.Users)
                  .HasForeignKey(e => e.RoleID);
        });
        base.OnModelCreating(modelBuilder);
    }
}

