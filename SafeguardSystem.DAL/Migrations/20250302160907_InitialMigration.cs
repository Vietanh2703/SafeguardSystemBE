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
                name: "SecurityGuards",
                columns: table => new
                {
                    GuardId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentityNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
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
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitude = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    BusinessId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PlaceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
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
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    GuardId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Teams_SecurityGuards_GuardId",
                        column: x => x.GuardId,
                        principalTable: "SecurityGuards",
                        principalColumn: "GuardId",
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
                    Latitude = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    LocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
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
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.ContractId);
                    table.ForeignKey(
                        name: "FK_Contracts_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
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
                    TypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                name: "ShiftAssignments",
                columns: table => new
                {
                    AssignmentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ShiftId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GuardId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LocationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CheckpointId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftAssignments", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_ShiftAssignments_Checkpoints_CheckpointId",
                        column: x => x.CheckpointId,
                        principalTable: "Checkpoints",
                        principalColumn: "CheckpointId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftAssignments_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftAssignments_SecurityGuards_GuardId",
                        column: x => x.GuardId,
                        principalTable: "SecurityGuards",
                        principalColumn: "GuardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftAssignments_SecurityShifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "SecurityShifts",
                        principalColumn: "ShiftId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ShiftIncidents",
                columns: table => new
                {
                    IncidentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AssignmentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Envidence = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncidentTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftIncidents", x => x.IncidentId);
                    table.ForeignKey(
                        name: "FK_ShiftIncidents_ShiftAssignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "ShiftAssignments",
                        principalColumn: "AssignmentId",
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
                    { new Guid("d1616b66-90cc-479f-b45e-1e86378937f7"), "Security Guard" }
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
                table: "Users",
                columns: new[] { "UserId", "ActivationToken", "ActivationTokenExpiry", "Avatar", "BirthDay", "Email", "FullName", "IsActive", "IsDeleted", "IsEmailConfirmed", "Phone", "ResetToken", "ResetTokenExpiry", "RoleID", "UserName" },
                values: new object[,]
                {
                    { "58sErANL7bbv096ghTnNN3qIiqX2", null, null, "https://www.freepik.com/free-vector/simple-vibing-cat-square-meme_58459053.htm#fromView=keyword&page=1&position=0&uuid=f4bd18ef-8de6-4b6e-8e68-06073abf526b&query=Animal+Memes", new DateTime(2004, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@test.com", "Viet Anh", true, false, true, "0123456789", null, null, new Guid("7a04e1d4-c176-467d-ac7d-6e1433ce6f3e"), "Admin" },
                    { "IuyPY3ie8OQG60w0gasxQNkXHzS2", null, null, "https://www.freepik.com/free-vector/simple-vibing-cat-square-meme_58459053.htm#fromView=keyword&page=1&position=0&uuid=f4bd18ef-8de6-4b6e-8e68-06073abf526b&query=Animal+Memes", new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "phong@test.com", "Nguyen Phuc Hau", true, false, true, "0123456789", null, null, new Guid("be19e4b3-6664-4afd-9ebb-98e0a073edc9"), "Business Partner" },
                    { "ksyosShXa2QizFvCVkpe6dAG3ax1", null, null, "https://www.freepik.com/free-vector/simple-vibing-cat-square-meme_58459053.htm#fromView=keyword&page=1&position=0&uuid=f4bd18ef-8de6-4b6e-8e68-06073abf526b&query=Animal+Memes", new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "guard@test.com", "Nguyen Khai Minh", true, false, true, "0123456789", null, null, new Guid("d1616b66-90cc-479f-b45e-1e86378937f7"), "Security Guard" },
                    { "UCdsPNEZpKeUlbPE8q478r60f5o1", null, null, "https://www.freepik.com/free-vector/simple-vibing-cat-square-meme_58459053.htm#fromView=keyword&page=1&position=0&uuid=f4bd18ef-8de6-4b6e-8e68-06073abf526b&query=Animal+Memes", new DateTime(2004, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "manager@test.com", "Nhat Nam", true, false, true, "0123456789", null, null, new Guid("6be95231-36aa-4a26-8c61-b65e040ec32a"), "Manager" }
                });

            migrationBuilder.InsertData(
                table: "Businesses",
                columns: new[] { "BusinessId", "ContractExpiry", "Description", "IsActive", "IsDeleted", "Name", "UserId" },
                values: new object[] { new Guid("7c54454f-337d-4b5b-a2d0-74ad4088686b"), new DateTime(2026, 3, 2, 16, 9, 3, 178, DateTimeKind.Utc).AddTicks(2365), "Nơi sinh hoạt văn hóa, giải trí dành cho sinh viên", true, false, "Nhà văn hóa sinh viên", "IuyPY3ie8OQG60w0gasxQNkXHzS2" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "BusinessId", "CreatedAt", "IsDeleted", "Latitude", "Longitude", "Name", "PlaceId", "UpdatedAt" },
                values: new object[] { new Guid("c86114da-36c8-4644-8ab0-dbdcb5c2f830"), new Guid("7c54454f-337d-4b5b-a2d0-74ad4088686b"), new DateTime(2025, 3, 2, 16, 9, 3, 178, DateTimeKind.Utc).AddTicks(2440), false, 10.882934m, 106.785746m, "Nhà văn hóa sinh viên", new Guid("07b246c8-222f-4cd5-a99b-757f754ea525"), null });

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_UserId",
                table: "Businesses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkpoints_LocationId",
                table: "Checkpoints",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_TeamId",
                table: "Contracts",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_BusinessId",
                table: "Locations",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Refreshtokens_UserId",
                table: "Refreshtokens",
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
                name: "IX_ShiftAssignments_CheckpointId",
                table: "ShiftAssignments",
                column: "CheckpointId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignments_GuardId",
                table: "ShiftAssignments",
                column: "GuardId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignments_LocationId",
                table: "ShiftAssignments",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignments_ShiftId",
                table: "ShiftAssignments",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftIncidents_AssignmentId",
                table: "ShiftIncidents",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_GuardId",
                table: "Teams",
                column: "GuardId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Refreshtokens");

            migrationBuilder.DropTable(
                name: "ShiftIncidents");

            migrationBuilder.DropTable(
                name: "ShiftAssignments");

            migrationBuilder.DropTable(
                name: "Checkpoints");

            migrationBuilder.DropTable(
                name: "SecurityShifts");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "ShiftTypes");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.DropTable(
                name: "SecurityGuards");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
