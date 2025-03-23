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

    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Business> Businesses { get; set; }
    public DbSet<Checkpoint> Checkpoints { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<LoginRequest> loginRequests { get; set; }
    public DbSet<RefreshToken> Refreshtokens { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<SecurityGuard> SecurityGuards { get; set; }
    public DbSet<SecurityShift> SecurityShifts { get; set; }
    public DbSet<ShiftType> ShiftTypes { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamGuard> TeamGuards { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Report> Reports { get; set; } // Add the Reports DbSet

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Seed();
        
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId);
            entity.HasOne(e => e.Shift)
                .WithOne()
                .HasForeignKey<Attendance>(e => e.ShiftId);
            entity.HasOne(e => e.Guard)
                .WithMany(g => g.Attendances)
                .HasForeignKey(e => e.GuardId);
        });
        
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
            entity.HasOne(e => e.Business)
                .WithMany(t => t.Contracts)
                .HasForeignKey(e => e.BusinessId);
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

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId);
            entity.Property(e => e.Name).IsRequired();
            entity.HasMany(e => e.TeamGuards)
                .WithOne(tg => tg.Team)
                .HasForeignKey(tg => tg.TeamId);
        });

        // Cấu hình bảng SecurityGuard
        modelBuilder.Entity<SecurityGuard>(entity =>
        {
            entity.HasKey(e => e.GuardId);
            entity.Property(e => e.IdentityNumber).IsRequired();
            entity.HasOne(e => e.User)
                .WithMany(u => u.SecurityGuards)
                .HasForeignKey(e => e.UserId);
            entity.HasMany(e => e.TeamGuards)
                .WithOne(tg => tg.Guard)
                .HasForeignKey(tg => tg.GuardId);
        });

        // Cấu hình bảng TeamGuard (Bảng trung gian)
        modelBuilder.Entity<TeamGuard>(entity =>
        {
            entity.HasKey(e => new { e.GuardId, e.TeamId }); // Khóa chính kép

            entity.HasOne(e => e.Guard)
                .WithMany(g => g.TeamGuards)
                .HasForeignKey(e => e.GuardId)
                .OnDelete(DeleteBehavior.Cascade); // Nếu xóa Guard, tự động xóa quan hệ trong TeamGuard

            entity.HasOne(e => e.Team)
                .WithMany(t => t.TeamGuards)
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.Cascade); // Nếu xóa Team, tự động xóa quan hệ trong TeamGuard
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


        modelBuilder.Entity<ShiftType>(entity =>
        {
            entity.HasKey(e => e.TypeId);
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.HasOne(e => e.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(e => e.RoleID);
        });

        modelBuilder.Entity<LoginRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId);
            entity.Property(e => e.Status).IsRequired();
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId);
            entity.Property(e => e.ReportComment).IsRequired();
            entity.HasOne(e => e.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(e => e.UserId);
        });


        base.OnModelCreating(modelBuilder);
    }
}