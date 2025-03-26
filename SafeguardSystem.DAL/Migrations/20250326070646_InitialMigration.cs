using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SafeguardSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ShiftTypes",
                columns: table => new
                {
                    TypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartTime = table.Column<TimeSpan>(type: "TIME(6)", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "TIME(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftTypes", x => x.TypeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Avatar = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gender = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkingContract = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BirthDay = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ActivationToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActivationTokenExpiry = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ResetToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResetTokenExpiry = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RoleID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    BusinessId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ContractExpiry = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.BusinessId);
                    table.ForeignKey(
                        name: "FK_Businesses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "loginRequests",
                columns: table => new
                {
                    RequestId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateSent = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateApproved = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Reason = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loginRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_loginRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Refreshtokens",
                columns: table => new
                {
                    RefreshTokenId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshTokenKey = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsRevoked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refreshtokens", x => x.RefreshTokenId);
                    table.ForeignKey(
                        name: "FK_Refreshtokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Sender = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Respondent = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReportComment = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Reason = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsClosed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AnsweredAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Reports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SecurityGuards",
                columns: table => new
                {
                    GuardId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentityNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityGuards", x => x.GuardId);
                    table.ForeignKey(
                        name: "FK_SecurityGuards_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    ContractId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ContractCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ContractValue = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BusinessId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.ContractId);
                    table.ForeignKey(
                        name: "FK_Contracts_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "BusinessId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Image = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitude = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    BusinessId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Locations_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "BusinessId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeamGuards",
                columns: table => new
                {
                    GuardId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TeamGuardId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamGuards", x => new { x.GuardId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_TeamGuards_SecurityGuards_GuardId",
                        column: x => x.GuardId,
                        principalTable: "SecurityGuards",
                        principalColumn: "GuardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamGuards_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Checkpoints",
                columns: table => new
                {
                    CheckpointId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkpoints", x => x.CheckpointId);
                    table.ForeignKey(
                        name: "FK_Checkpoints_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SecurityShifts",
                columns: table => new
                {
                    ShiftId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ShiftDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityShifts", x => x.ShiftId);
                    table.ForeignKey(
                        name: "FK_SecurityShifts_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecurityShifts_ShiftTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ShiftTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecurityShifts_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ShiftId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GuardId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CheckInDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CheckInTime = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendanceId);
                    table.ForeignKey(
                        name: "FK_Attendances_SecurityGuards_GuardId",
                        column: x => x.GuardId,
                        principalTable: "SecurityGuards",
                        principalColumn: "GuardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_SecurityShifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "SecurityShifts",
                        principalColumn: "ShiftId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { new Guid("6be95231-36aa-4a26-8c61-b65e040ec32a"), "Manager" },
                    { new Guid("7a04e1d4-c176-467d-ac7d-6e1433ce6f3e"), "Admin" },
                    { new Guid("be19e4b3-6664-4afd-9ebb-98e0a073edc9"), "Business Partner" },
                    { new Guid("d1616b66-90cc-479f-b45e-1e86378937f7"), "Security Guard" },
                    { new Guid("d7b7ef50-cec3-4089-ac49-456001fc43a6"), "Processing" }
                });

            migrationBuilder.InsertData(
                table: "ShiftTypes",
                columns: new[] { "TypeId", "Description", "EndTime", "IsDeleted", "Name", "StartTime" },
                values: new object[,]
                {
                    { new Guid("4112683c-a85f-4429-b6ac-292ca9364155"), "Morning shift", new TimeSpan(0, 12, 0, 0, 0), false, "Morning", new TimeSpan(0, 5, 0, 0, 0) },
                    { new Guid("c333cd26-e186-445b-969f-e2fc983b18b8"), "Afternoon shift", new TimeSpan(0, 19, 0, 0, 0), false, "Afternoon", new TimeSpan(0, 12, 0, 0, 0) },
                    { new Guid("cdbb6b9e-c537-4659-b909-60712082038d"), "Night shift", new TimeSpan(0, 2, 0, 0, 0), false, "Night", new TimeSpan(0, 19, 0, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "TeamId", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { new Guid("61ac7456-620f-4ab3-95f5-616c8d42f68b"), false, "Team 3" },
                    { new Guid("99f6ff03-bc67-429a-8b21-25bcdbb6e6fc"), false, "Team 2" },
                    { new Guid("c34241ec-93db-4fae-9b2d-2c2f25e08c40"), false, "Team 1" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "ActivationToken", "ActivationTokenExpiry", "Address", "Avatar", "BirthDay", "Email", "FullName", "Gender", "IsActive", "IsDeleted", "IsEmailConfirmed", "IsLocked", "Phone", "ResetToken", "ResetTokenExpiry", "RoleID", "UserName", "WorkingContract" },
                values: new object[,]
                {
                    { "58sErANL7bbv096ghTnNN3qIiqX2", null, null, "Ho Chi Minh City", "https://img.freepik.com/free-vector/simple-vibing-cat-square-meme_742173-4493.jpg?t=st=1741640485~exp=1741644085~hmac=2b51c5540d42bb03d640ddf64d9a7d048ebb8a62f836c15ae349ca193c7cce01&w=900", new DateTime(2004, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@test.com", "Viet Anh", "Male", true, false, true, false, "0123456789", null, null, new Guid("7a04e1d4-c176-467d-ac7d-6e1433ce6f3e"), "Admin", "Full-time" },
                    { "8RIijLXzhAfpXHOna9T2JWctYSE3", null, null, "Ho Chi Minh City", "https://img.freepik.com/free-vector/simple-vibing-cat-square-meme_742173-4493.jpg?t=st=1741640485~exp=1741644085~hmac=2b51c5540d42bb03d640ddf64d9a7d048ebb8a62f836c15ae349ca193c7cce01&w=900", new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "guard@test.com", "Nguyen Khai Minh", "Male", true, false, true, false, "0123456789", null, null, new Guid("d1616b66-90cc-479f-b45e-1e86378937f7"), "Security Guard", "Full-time" },
                    { "CkjtbJVJQxVm1eLjHW3p10TdV193", null, null, "Ho Chi Minh City", "https://img.freepik.com/free-vector/simple-vibing-cat-square-meme_742173-4493.jpg?t=st=1741640485~exp=1741644085~hmac=2b51c5540d42bb03d640ddf64d9a7d048ebb8a62f836c15ae349ca193c7cce01&w=900", new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "guard3@test.com", "Nguyen Hieu", "Male", true, false, true, false, "0123456789", null, null, new Guid("d1616b66-90cc-479f-b45e-1e86378937f7"), "Security Guard 3", "Full-time" },
                    { "fH8JsAPWjJOHLvLSI4MJVG4aSBr1", null, null, "Ho Chi Minh City", "https://img.freepik.com/free-vector/simple-vibing-cat-square-meme_742173-4493.jpg?t=st=1741640485~exp=1741644085~hmac=2b51c5540d42bb03d640ddf64d9a7d048ebb8a62f836c15ae349ca193c7cce01&w=900", new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "guard2@test.com", "Nguyen Huy", "Male", true, false, true, false, "0123456789", null, null, new Guid("d1616b66-90cc-479f-b45e-1e86378937f7"), "Security Guard 2", "Full-time" },
                    { "fNBgIDvy0JTd3wd9enqVywR8o612", null, null, "Ho Chi Minh City", "https://img.freepik.com/free-vector/simple-vibing-cat-square-meme_742173-4493.jpg?t=st=1741640485~exp=1741644085~hmac=2b51c5540d42bb03d640ddf64d9a7d048ebb8a62f836c15ae349ca193c7cce01&w=900", new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "guard5@test.com", "Nguyen Dang", "Male", true, false, true, false, "0123456789", null, null, new Guid("d1616b66-90cc-479f-b45e-1e86378937f7"), "Security Guard 5", "Full-time" },
                    { "GLgL2MXjY7Pg4gmEJrgFulyEgV23", null, null, "Ho Chi Minh City", "https://img.freepik.com/free-vector/simple-vibing-cat-square-meme_742173-4493.jpg?t=st=1741640485~exp=1741644085~hmac=2b51c5540d42bb03d640ddf64d9a7d048ebb8a62f836c15ae349ca193c7cce01&w=900", new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "guard4@test.com", "Nguyen Phong", "Male", true, false, true, true, "0123456789", null, null, new Guid("d1616b66-90cc-479f-b45e-1e86378937f7"), "Security Guard 4", "Full-time" },
                    { "SLuhlRSnI1VOm00lYZ0oHxnfpCx2", null, null, "Ho Chi Minh City", "https://img.freepik.com/free-vector/simple-vibing-cat-square-meme_742173-4493.jpg?t=st=1741640485~exp=1741644085~hmac=2b51c5540d42bb03d640ddf64d9a7d048ebb8a62f836c15ae349ca193c7cce01&w=900", new DateTime(2004, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "manager@test.com", "Nhat Nam", "Male", true, false, true, false, "0123456789", null, null, new Guid("6be95231-36aa-4a26-8c61-b65e040ec32a"), "Manager", "Full-time" },
                    { "zJofxJXGJvStWSLCCJVoGz8uXsf2", null, null, "Ho Chi Minh City", "https://img.freepik.com/free-vector/simple-vibing-cat-square-meme_742173-4493.jpg?t=st=1741640485~exp=1741644085~hmac=2b51c5540d42bb03d640ddf64d9a7d048ebb8a62f836c15ae349ca193c7cce01&w=900", new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "business@test.com", "Nguyen Phuc Hau", "Male", true, false, true, false, "0123456789", null, null, new Guid("be19e4b3-6664-4afd-9ebb-98e0a073edc9"), "Business Partner", "Full-time" }
                });

            migrationBuilder.InsertData(
                table: "Businesses",
                columns: new[] { "BusinessId", "ContractExpiry", "Description", "IsActive", "IsDeleted", "Name", "UserId" },
                values: new object[] { new Guid("7c54454f-337d-4b5b-a2d0-74ad4088686b"), new DateTime(2026, 3, 26, 7, 6, 37, 284, DateTimeKind.Utc).AddTicks(9392), "Nơi sinh hoạt văn hóa, giải trí dành cho sinh viên", true, false, "Nhà văn hóa sinh viên", "zJofxJXGJvStWSLCCJVoGz8uXsf2" });

            migrationBuilder.InsertData(
                table: "SecurityGuards",
                columns: new[] { "GuardId", "IdentityNumber", "StartDate", "Status", "UserId" },
                values: new object[,]
                {
                    { new Guid("399c04bc-31f2-4296-bf32-4b6d3b0cb1c1"), "123456789", new DateTime(2025, 3, 26, 7, 6, 37, 284, DateTimeKind.Utc).AddTicks(9323), "PENDING", "fH8JsAPWjJOHLvLSI4MJVG4aSBr1" },
                    { new Guid("3de02800-eecb-4abe-adb4-cd114896e0bc"), "123456789", new DateTime(2025, 3, 26, 7, 6, 37, 284, DateTimeKind.Utc).AddTicks(9329), "PENDING", "fNBgIDvy0JTd3wd9enqVywR8o612" },
                    { new Guid("5144d02b-8aa2-43d1-88e9-774087acb37e"), "123456789", new DateTime(2025, 3, 26, 7, 6, 37, 284, DateTimeKind.Utc).AddTicks(9327), "PENDING", "GLgL2MXjY7Pg4gmEJrgFulyEgV23" },
                    { new Guid("d2a9201b-59ad-40b7-bde5-dfda937d7433"), "123456789", new DateTime(2025, 3, 26, 7, 6, 37, 284, DateTimeKind.Utc).AddTicks(9319), "PENDING", "8RIijLXzhAfpXHOna9T2JWctYSE3" },
                    { new Guid("e24aa81d-c60e-4dfd-9909-991c837e6631"), "123456789", new DateTime(2025, 3, 26, 7, 6, 37, 284, DateTimeKind.Utc).AddTicks(9325), "PENDING", "CkjtbJVJQxVm1eLjHW3p10TdV193" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Address", "BusinessId", "CreatedAt", "Image", "IsDeleted", "Latitude", "Longitude", "Name", "UpdatedAt" },
                values: new object[] { new Guid("c86114da-36c8-4644-8ab0-dbdcb5c2f830"), "Khu phố 6, Phường Linh Trung, Thủ Đức, Thành phố Hồ Chí Minh", new Guid("7c54454f-337d-4b5b-a2d0-74ad4088686b"), new DateTime(2025, 3, 26, 7, 6, 37, 284, DateTimeKind.Utc).AddTicks(9435), "https://www.freepik.com/free-vector/simple-vibing-cat-square-meme_58459053.htm#fromView=keyword&page=1&position=0&uuid=f4bd18ef-8de6-4b6e-8e68-06073abf526b&query=Animal+Memes", false, 10.8751312m, 106.8007233m, "Nhà văn hóa sinh viên", null });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_GuardId",
                table: "Attendances",
                column: "GuardId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ShiftId",
                table: "Attendances",
                column: "ShiftId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_UserId",
                table: "Businesses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkpoints_LocationId",
                table: "Checkpoints",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_BusinessId",
                table: "Contracts",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_BusinessId",
                table: "Locations",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_loginRequests_UserId",
                table: "loginRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Refreshtokens_UserId",
                table: "Refreshtokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId",
                table: "Reports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGuards_UserId",
                table: "SecurityGuards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityShifts_LocationId",
                table: "SecurityShifts",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityShifts_TeamId",
                table: "SecurityShifts",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityShifts_TypeId",
                table: "SecurityShifts",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamGuards_TeamId",
                table: "TeamGuards",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Checkpoints");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "loginRequests");

            migrationBuilder.DropTable(
                name: "Refreshtokens");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "TeamGuards");

            migrationBuilder.DropTable(
                name: "SecurityShifts");

            migrationBuilder.DropTable(
                name: "SecurityGuards");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "ShiftTypes");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
