using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using SafeguardSystem.DAL.Entities;

namespace SafeguardSystem.DAL.DBContext;

public partial class SafeguardDbContext : DbContext
{
    public SafeguardDbContext()
    {
    }

    public SafeguardDbContext(DbContextOptions<SafeguardDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<Checkpoint> Checkpoints { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<Levelincident> Levelincidents { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Securityguard> Securityguards { get; set; }

    public virtual DbSet<Securityshift> Securityshifts { get; set; }

    public virtual DbSet<Shiftassignment> Shiftassignments { get; set; }

    public virtual DbSet<Shiftincident> Shiftincidents { get; set; }

    public virtual DbSet<Shifttype> Shifttypes { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(GetConnectionString("DefaultConnection"))).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseMySql("server=localhost;user=root;password=12345;database=safeguard_db", ServerVersion.Parse("8.0.41-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.BusinessId).HasName("PRIMARY");

            entity.ToTable("business");

            entity.HasIndex(e => e.UserId, "userId");

            entity.Property(e => e.BusinessId).HasColumnName("businessId");
            entity.Property(e => e.ContractExpiry)
                .HasColumnType("datetime")
                .HasColumnName("contractExpiry");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasColumnType("bit(1)")
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isDeleted");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Businesses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("business_ibfk_1");
        });

        modelBuilder.Entity<Checkpoint>(entity =>
        {
            entity.HasKey(e => e.CheckpointId).HasName("PRIMARY");

            entity.ToTable("checkpoint");

            entity.HasIndex(e => e.LocationId, "locationId");

            entity.Property(e => e.CheckpointId).HasColumnName("checkpointId");
            entity.Property(e => e.IsDeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isDeleted");
            entity.Property(e => e.Latitude)
                .HasPrecision(10, 8)
                .HasColumnName("latitude");
            entity.Property(e => e.LocationId).HasColumnName("locationId");
            entity.Property(e => e.Longitude)
                .HasPrecision(11, 8)
                .HasColumnName("longitude");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PlaceId).HasColumnName("placeId");

            entity.HasOne(d => d.Location).WithMany(p => p.Checkpoints)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("checkpoint_ibfk_1");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PRIMARY");

            entity.ToTable("contract");

            entity.HasIndex(e => e.TeamId, "teamId");

            entity.Property(e => e.ContractId).HasColumnName("contractId");
            entity.Property(e => e.ContractCode)
                .HasMaxLength(255)
                .HasColumnName("contractCode");
            entity.Property(e => e.ContractValue)
                .HasPrecision(15, 2)
                .HasColumnName("contractValue");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.IsDeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isDeleted");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TeamId).HasColumnName("teamId");

            entity.HasOne(d => d.Team).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contract_ibfk_1");
        });

        modelBuilder.Entity<Levelincident>(entity =>
        {
            entity.HasKey(e => e.LevelId).HasName("PRIMARY");

            entity.ToTable("levelincident");

            entity.Property(e => e.LevelId).HasColumnName("levelId");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PRIMARY");

            entity.ToTable("location");

            entity.HasIndex(e => e.BusinessId, "businessId");

            entity.Property(e => e.LocationId).HasColumnName("locationId");
            entity.Property(e => e.BusinessId).HasColumnName("businessId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.IsDeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isDeleted");
            entity.Property(e => e.Latitude)
                .HasPrecision(10, 8)
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasPrecision(11, 8)
                .HasColumnName("longitude");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PlaceId).HasColumnName("placeId");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Business).WithMany(p => p.Locations)
                .HasForeignKey(d => d.BusinessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("location_ibfk_1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<Securityguard>(entity =>
        {
            entity.HasKey(e => e.GuardId).HasName("PRIMARY");

            entity.ToTable("securityguard");

            entity.HasIndex(e => e.TeamId, "teamId");

            entity.HasIndex(e => e.UserId, "userId");

            entity.Property(e => e.GuardId).HasColumnName("guardId");
            entity.Property(e => e.IdentityNumber)
                .HasMaxLength(50)
                .HasColumnName("identityNumber");
            entity.Property(e => e.Latitude)
                .HasPrecision(10, 8)
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasPrecision(11, 8)
                .HasColumnName("longitude");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("startDate");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TeamId).HasColumnName("teamId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Team).WithMany(p => p.Securityguards)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("securityguard_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.Securityguards)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("securityguard_ibfk_2");
        });

        modelBuilder.Entity<Securityshift>(entity =>
        {
            entity.HasKey(e => e.ShiftId).HasName("PRIMARY");

            entity.ToTable("securityshift");

            entity.HasIndex(e => e.LocationId, "locationId");

            entity.HasIndex(e => e.TeamId, "teamId");

            entity.HasIndex(e => e.TypeId, "typeId");

            entity.Property(e => e.ShiftId).HasColumnName("shiftId");
            entity.Property(e => e.LocationId).HasColumnName("locationId");
            entity.Property(e => e.TeamId).HasColumnName("teamId");
            entity.Property(e => e.TypeId).HasColumnName("typeId");

            entity.HasOne(d => d.Location).WithMany(p => p.Securityshifts)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("securityshift_ibfk_1");

            entity.HasOne(d => d.Team).WithMany(p => p.Securityshifts)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("securityshift_ibfk_3");

            entity.HasOne(d => d.Type).WithMany(p => p.Securityshifts)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("securityshift_ibfk_2");
        });

        modelBuilder.Entity<Shiftassignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PRIMARY");

            entity.ToTable("shiftassignment");

            entity.HasIndex(e => e.CheckpointId, "checkpointId");

            entity.HasIndex(e => e.GuardId, "guardId");

            entity.HasIndex(e => e.LocationId, "locationId");

            entity.HasIndex(e => e.ShiftId, "shiftId");

            entity.Property(e => e.AssignmentId).HasColumnName("assignmentId");
            entity.Property(e => e.CheckpointId).HasColumnName("checkpointId");
            entity.Property(e => e.GuardId).HasColumnName("guardId");
            entity.Property(e => e.LocationId).HasColumnName("locationId");
            entity.Property(e => e.ShiftId).HasColumnName("shiftId");

            entity.HasOne(d => d.Checkpoint).WithMany(p => p.Shiftassignments)
                .HasForeignKey(d => d.CheckpointId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shiftassignment_ibfk_4");

            entity.HasOne(d => d.Guard).WithMany(p => p.Shiftassignments)
                .HasForeignKey(d => d.GuardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shiftassignment_ibfk_2");

            entity.HasOne(d => d.Location).WithMany(p => p.Shiftassignments)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shiftassignment_ibfk_3");

            entity.HasOne(d => d.Shift).WithMany(p => p.Shiftassignments)
                .HasForeignKey(d => d.ShiftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shiftassignment_ibfk_1");
        });

        modelBuilder.Entity<Shiftincident>(entity =>
        {
            entity.HasKey(e => e.IncidentId).HasName("PRIMARY");

            entity.ToTable("shiftincident");

            entity.HasIndex(e => e.AssignmentId, "assignmentId");

            entity.HasIndex(e => e.LevelId, "levelId");

            entity.Property(e => e.IncidentId).HasColumnName("incidentId");
            entity.Property(e => e.AssignmentId).HasColumnName("assignmentId");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Envidence).HasColumnName("envidence");
            entity.Property(e => e.IncidentTime)
                .HasColumnType("datetime")
                .HasColumnName("incidentTime");
            entity.Property(e => e.IsDeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isDeleted");
            entity.Property(e => e.LevelId).HasColumnName("levelId");

            entity.HasOne(d => d.Assignment).WithMany(p => p.Shiftincidents)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shiftincident_ibfk_1");

            entity.HasOne(d => d.Level).WithMany(p => p.Shiftincidents)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shiftincident_ibfk_2");
        });

        modelBuilder.Entity<Shifttype>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PRIMARY");

            entity.ToTable("shifttype");

            entity.Property(e => e.TypeId).HasColumnName("typeId");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("endTime");
            entity.Property(e => e.IsDeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isDeleted");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("startTime");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PRIMARY");

            entity.ToTable("team");

            entity.Property(e => e.TeamId).HasColumnName("teamId");
            entity.Property(e => e.IsDeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isDeleted");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.ActivationToken).HasColumnName("activationToken");
            entity.Property(e => e.ActivationTokenExpiry)
                .HasColumnType("datetime")
                .HasColumnName("activationTokenExpiry");
            entity.Property(e => e.Avatar).HasColumnName("avatar");
            entity.Property(e => e.BirthDay)
                .HasColumnType("datetime")
                .HasColumnName("birthDay");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("fullName");
            entity.Property(e => e.IsActive)
                .HasColumnType("bit(1)")
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isDeleted");
            entity.Property(e => e.IsEmailConfirmed)
                .HasColumnType("bit(1)")
                .HasColumnName("isEmailConfirmed");
            entity.Property(e => e.PasswordHash).HasColumnName("passwordHash");
            entity.Property(e => e.PasswordSalt).HasColumnName("passwordSalt");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.ResetToken).HasColumnName("resetToken");
            entity.Property(e => e.ResetTokenExpiry)
                .HasColumnType("datetime")
                .HasColumnName("resetTokenExpiry");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("updateAt");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("userName");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Userrole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("userrole_ibfk_2"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("userrole_ibfk_1"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("userrole");
                        j.HasIndex(new[] { "RoleId" }, "roleId");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("userId");
                        j.IndexerProperty<Guid>("RoleId").HasColumnName("roleId");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
