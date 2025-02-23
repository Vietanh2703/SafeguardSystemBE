using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;

namespace SafeguardSystem.DAL;

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

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<RefreshToken> Refreshtokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SecurityGuard> SecurityGuards { get; set; }

    public virtual DbSet<SecurityShift> SecurityShifts { get; set; }

    public virtual DbSet<ShiftAssignment> ShiftAssignments { get; set; }

    public virtual DbSet<ShiftIncident> ShiftIncidents { get; set; }

    public virtual DbSet<ShiftType> ShiftTypes { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Seed();

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.BusinessId).HasName("PRIMARY");

            entity.ToTable("business");

            entity.HasIndex(e => e.UserId, "userId");

            entity.Property(e => e.BusinessId)
                .HasColumnName("businessId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
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
            entity.Property(e => e.UserId)
                .HasColumnName("userId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

            entity.HasOne(d => d.User).WithMany(p => p.Businesses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("business_ibfk_1");
        });

        modelBuilder.Entity<Checkpoint>(entity =>
        {
            entity.HasKey(e => e.CheckpointId).HasName("PRIMARY");

            entity.ToTable("checkpoint");

            entity.HasIndex(e => e.LocationId, "locationId");

            entity.Property(e => e.CheckpointId)
                .HasColumnName("checkpointId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.IsDeleted)
                .HasColumnType("bit(1)")
                .HasColumnName("isDeleted");
            entity.Property(e => e.Latitude)
                .HasPrecision(10, 8)
                .HasColumnName("latitude");
            entity.Property(e => e.LocationId)
                .HasColumnName("locationId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.Longitude)
                .HasPrecision(11, 8)
                .HasColumnName("longitude");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Location).WithMany(p => p.Checkpoints)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("checkpoint_ibfk_1");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PRIMARY");

            entity.ToTable("contract");

            entity.HasIndex(e => e.TeamId, "teamId");

            entity.Property(e => e.ContractId)
                .HasColumnName("contractId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
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
            entity.Property(e => e.TeamId)
                .HasColumnName("teamId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

            entity.HasOne(d => d.Team).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("contract_ibfk_1");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PRIMARY");

            entity.ToTable("location");

            entity.HasIndex(e => e.BusinessId, "businessId");

            entity.Property(e => e.LocationId)
                .HasColumnName("locationId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.BusinessId)
                .HasColumnName("businessId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
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
            entity.Property(e => e.PlaceId)
                .HasColumnName("placeId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Business).WithMany(p => p.Locations)
                .HasForeignKey(d => d.BusinessId)
                .HasConstraintName("location_ibfk_1");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenId).HasName("PRIMARY");

            entity.ToTable("refreshtoken");

            entity.HasIndex(e => e.UserId, "userId1");

            entity.Property(e => e.RefreshTokenId)
                .HasColumnName("refreshTokenId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("createAt");
            entity.Property(e => e.IsRevoked)
                .HasColumnType("bit(1)")
                .HasColumnName("isRevoked");
            entity.Property(e => e.RefreshTokenKey).HasColumnName("refreshTokenKey");
            entity.Property(e => e.UserId)
                .HasColumnName("userId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("refreshtoken_ibfk_1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId)
                .HasColumnName("roleId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<SecurityGuard>(entity =>
        {
            entity.HasKey(e => e.GuardId).HasName("PRIMARY");

            entity.ToTable("securityguard");

            entity.HasIndex(e => e.UserId, "userId2");

            entity.Property(e => e.GuardId)
                .HasColumnName("guardId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
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
            entity.Property(e => e.UserId)
                .HasColumnName("userId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

            entity.HasOne(d => d.User).WithMany(p => p.SecurityGuards)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("securityguard_ibfk_2");
        });

        modelBuilder.Entity<SecurityShift>(entity =>
        {
            entity.HasKey(e => e.ShiftId).HasName("PRIMARY");

            entity.ToTable("securityshift");

            entity.HasIndex(e => e.LocationId, "locationId1");

            entity.HasIndex(e => e.TeamId, "teamId1");

            entity.HasIndex(e => e.TypeId, "typeId");

            entity.Property(e => e.ShiftId)
                .HasColumnName("shiftId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.LocationId)
                .HasColumnName("locationId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.TeamId)
                .HasColumnName("teamId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.TypeId)
                .HasColumnName("typeId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

            entity.HasOne(d => d.Location).WithMany(p => p.SecurityShifts)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("securityshift_ibfk_1");

            entity.HasOne(d => d.Team).WithMany(p => p.SecurityShifts)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("securityshift_ibfk_3");

            entity.HasOne(d => d.Type).WithMany(p => p.SecurityShifts)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("securityshift_ibfk_2");
        });

        modelBuilder.Entity<ShiftAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PRIMARY");

            entity.ToTable("shiftassignment");

            entity.HasIndex(e => e.CheckpointId, "checkpointId");

            entity.HasIndex(e => e.GuardId, "guardId");

            entity.HasIndex(e => e.LocationId, "locationId2");

            entity.HasIndex(e => e.ShiftId, "shiftId");

            entity.Property(e => e.AssignmentId)
                .HasColumnName("assignmentId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.CheckpointId)
                .HasColumnName("checkpointId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.GuardId)
                .HasColumnName("guardId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.LocationId)
                .HasColumnName("locationId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ShiftId)
                .HasColumnName("shiftId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

            entity.HasOne(d => d.Checkpoint).WithMany(p => p.ShiftAssignments)
                .HasForeignKey(d => d.CheckpointId)
                .HasConstraintName("shiftassignment_ibfk_4");

            entity.HasOne(d => d.Guard).WithMany(p => p.ShiftAssignments)
                .HasForeignKey(d => d.GuardId)
                .HasConstraintName("shiftassignment_ibfk_2");

            entity.HasOne(d => d.Location).WithMany(p => p.ShiftAssignments)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("shiftassignment_ibfk_3");

            entity.HasOne(d => d.Shift).WithMany(p => p.ShiftAssignments)
                .HasForeignKey(d => d.ShiftId)
                .HasConstraintName("shiftassignment_ibfk_1");
        });

        modelBuilder.Entity<ShiftIncident>(entity =>
        {
            entity.HasKey(e => e.IncidentId).HasName("PRIMARY");

            entity.ToTable("shiftincident");

            entity.HasIndex(e => e.AssignmentId, "assignmentId");

            entity.Property(e => e.IncidentId)
                .HasColumnName("incidentId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.AssignmentId)
                .HasColumnName("assignmentId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
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

            entity.HasOne(d => d.Assignment).WithMany(p => p.ShiftIncidents)
                .HasForeignKey(d => d.AssignmentId)
                .HasConstraintName("shiftincident_ibfk_1");
        });

        modelBuilder.Entity<ShiftType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PRIMARY");

            entity.ToTable("shifttype");

            entity.Property(e => e.TypeId)
                .HasColumnName("typeId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
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

            entity.HasIndex(e => e.GuardId, "IX_team_GuardId");

            entity.Property(e => e.TeamId)
                .HasColumnName("teamId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.GuardId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
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

            entity.HasIndex(e => e.RoleID, "IX_users_RoleID");

            entity.Property(e => e.UserId)
                .HasColumnName("userId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
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
            entity.Property(e => e.RoleID)
                .HasColumnName("RoleID")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("userName");

            entity.HasOne(d => d.Role).WithMany(p => p.Users).HasForeignKey(d => d.RoleID);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

