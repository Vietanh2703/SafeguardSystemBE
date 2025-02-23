using System;
using Microsoft.EntityFrameworkCore.Metadata;
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
                name: "roles",
                columns: table => new
                {
                    roleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    roleName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.roleId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shifttype",
                columns: table => new
                {
                    typeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    startTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    endTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    isDeleted = table.Column<ulong>(type: "bit(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.typeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    userName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fullName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avatar = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    birthDay = table.Column<DateTime>(type: "datetime", nullable: true),
                    passwordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    passwordSalt = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    activationToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    activationTokenExpiry = table.Column<DateTime>(type: "datetime", nullable: true),
                    resetToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    resetTokenExpiry = table.Column<DateTime>(type: "datetime", nullable: true),
                    isActive = table.Column<ulong>(type: "bit(1)", nullable: false),
                    isEmailConfirmed = table.Column<ulong>(type: "bit(1)", nullable: false),
                    isDeleted = table.Column<ulong>(type: "bit(1)", nullable: false),
                    RoleID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.userId);
                    table.ForeignKey(
                        name: "FK_users_roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "roles",
                        principalColumn: "roleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "business",
                columns: table => new
                {
                    businessId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    isActive = table.Column<ulong>(type: "bit(1)", nullable: false),
                    contractExpiry = table.Column<DateTime>(type: "datetime", nullable: false),
                    isDeleted = table.Column<ulong>(type: "bit(1)", nullable: false),
                    userId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.businessId);
                    table.ForeignKey(
                        name: "business_ibfk_1",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "refreshtoken",
                columns: table => new
                {
                    refreshTokenId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    userId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    refreshTokenKey = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    isRevoked = table.Column<ulong>(type: "bit(1)", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.refreshTokenId);
                    table.ForeignKey(
                        name: "refreshtoken_ibfk_1",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "securityguard",
                columns: table => new
                {
                    guardId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    identityNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    startDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    latitude = table.Column<decimal>(type: "decimal(10,8)", precision: 10, scale: 8, nullable: false),
                    longitude = table.Column<decimal>(type: "decimal(11,8)", precision: 11, scale: 8, nullable: false),
                    userId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.guardId);
                    table.ForeignKey(
                        name: "securityguard_ibfk_2",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "location",
                columns: table => new
                {
                    locationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    latitude = table.Column<decimal>(type: "decimal(10,8)", precision: 10, scale: 8, nullable: false),
                    longitude = table.Column<decimal>(type: "decimal(11,8)", precision: 11, scale: 8, nullable: false),
                    businessId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    placeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    isDeleted = table.Column<ulong>(type: "bit(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.locationId);
                    table.ForeignKey(
                        name: "location_ibfk_1",
                        column: x => x.businessId,
                        principalTable: "business",
                        principalColumn: "businessId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "team",
                columns: table => new
                {
                    teamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    isDeleted = table.Column<ulong>(type: "bit(1)", nullable: false),
                    GuardId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.teamId);
                    table.ForeignKey(
                        name: "FK_team_securityguard_GuardId",
                        column: x => x.GuardId,
                        principalTable: "securityguard",
                        principalColumn: "guardId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "checkpoint",
                columns: table => new
                {
                    checkpointId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    latitude = table.Column<decimal>(type: "decimal(10,8)", precision: 10, scale: 8, nullable: false),
                    longitude = table.Column<decimal>(type: "decimal(11,8)", precision: 11, scale: 8, nullable: false),
                    locationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    isDeleted = table.Column<ulong>(type: "bit(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.checkpointId);
                    table.ForeignKey(
                        name: "checkpoint_ibfk_1",
                        column: x => x.locationId,
                        principalTable: "location",
                        principalColumn: "locationId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "contract",
                columns: table => new
                {
                    contractId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    contractCode = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    startDate = table.Column<DateOnly>(type: "date", nullable: false),
                    endDate = table.Column<DateOnly>(type: "date", nullable: false),
                    contractValue = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    teamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    isDeleted = table.Column<ulong>(type: "bit(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.contractId);
                    table.ForeignKey(
                        name: "contract_ibfk_1",
                        column: x => x.teamId,
                        principalTable: "team",
                        principalColumn: "teamId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "securityshift",
                columns: table => new
                {
                    shiftId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    locationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    teamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    typeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.shiftId);
                    table.ForeignKey(
                        name: "securityshift_ibfk_1",
                        column: x => x.locationId,
                        principalTable: "location",
                        principalColumn: "locationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "securityshift_ibfk_2",
                        column: x => x.typeId,
                        principalTable: "shifttype",
                        principalColumn: "typeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "securityshift_ibfk_3",
                        column: x => x.teamId,
                        principalTable: "team",
                        principalColumn: "teamId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shiftassignment",
                columns: table => new
                {
                    assignmentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    shiftId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    guardId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    locationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    checkpointId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.assignmentId);
                    table.ForeignKey(
                        name: "shiftassignment_ibfk_1",
                        column: x => x.shiftId,
                        principalTable: "securityshift",
                        principalColumn: "shiftId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "shiftassignment_ibfk_2",
                        column: x => x.guardId,
                        principalTable: "securityguard",
                        principalColumn: "guardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "shiftassignment_ibfk_3",
                        column: x => x.locationId,
                        principalTable: "location",
                        principalColumn: "locationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "shiftassignment_ibfk_4",
                        column: x => x.checkpointId,
                        principalTable: "checkpoint",
                        principalColumn: "checkpointId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shiftincident",
                columns: table => new
                {
                    incidentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    assignmentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    envidence = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    incidentTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    isDeleted = table.Column<ulong>(type: "bit(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.incidentId);
                    table.ForeignKey(
                        name: "shiftincident_ibfk_1",
                        column: x => x.assignmentId,
                        principalTable: "shiftassignment",
                        principalColumn: "assignmentId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "roleId", "roleName" },
                values: new object[,]
                {
                    { new Guid("6be95231-36aa-4a26-8c61-b65e040ec32a"), "Manager" },
                    { new Guid("7a04e1d4-c176-467d-ac7d-6e1433ce6f3e"), "Admin" },
                    { new Guid("be19e4b3-6664-4afd-9ebb-98e0a073edc9"), "Business partner" },
                    { new Guid("d1616b66-90cc-479f-b45e-1e86378937f7"), "Guard" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "userId", "activationToken", "activationTokenExpiry", "avatar", "birthDay", "email", "fullName", "isActive", "isDeleted", "isEmailConfirmed", "passwordHash", "passwordSalt", "phone", "resetToken", "resetTokenExpiry", "RoleID", "userName" },
                values: new object[,]
                {
                    { new Guid("62f56fcb-bcd3-474a-b938-90bda6af54d3"), null, null, "https://www.freepik.com/free-vector/simple-vibing-cat-square-meme_58459053.htm#fromView=keyword&page=1&position=0&uuid=f4bd18ef-8de6-4b6e-8e68-06073abf526b&query=Animal+Memes", new DateTime(2004, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "nam@gmail.com", "Nhat Nam", 1ul, 0ul, 1ul, "$2a$12$2slGRhLPjB/yiLGpmhxHNOdayMV1MXV/L6EeK3M/VEE04QK31/rcq", "", "0123456789", null, null, new Guid("6be95231-36aa-4a26-8c61-b65e040ec32a"), "Manager" },
                    { new Guid("d71a9f93-2bc9-4771-9732-8ee036601ba5"), null, null, "https://www.freepik.com/free-vector/simple-vibing-cat-square-meme_58459053.htm#fromView=keyword&page=1&position=0&uuid=f4bd18ef-8de6-4b6e-8e68-06073abf526b&query=Animal+Memes", new DateTime(2004, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "a@gmail.com", "Viet Anh", 1ul, 0ul, 1ul, "$2a$12$wfFP3IPQecfFr1JruMqzae0Z1Mexhvtq/Lhw9luEYNw.0dD/TTzau", "", "0123456789", null, null, new Guid("7a04e1d4-c176-467d-ac7d-6e1433ce6f3e"), "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "userId",
                table: "business",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "locationId",
                table: "checkpoint",
                column: "locationId");

            migrationBuilder.CreateIndex(
                name: "teamId",
                table: "contract",
                column: "teamId");

            migrationBuilder.CreateIndex(
                name: "businessId",
                table: "location",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "userId1",
                table: "refreshtoken",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "userId2",
                table: "securityguard",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "locationId1",
                table: "securityshift",
                column: "locationId");

            migrationBuilder.CreateIndex(
                name: "teamId1",
                table: "securityshift",
                column: "teamId");

            migrationBuilder.CreateIndex(
                name: "typeId",
                table: "securityshift",
                column: "typeId");

            migrationBuilder.CreateIndex(
                name: "checkpointId",
                table: "shiftassignment",
                column: "checkpointId");

            migrationBuilder.CreateIndex(
                name: "guardId",
                table: "shiftassignment",
                column: "guardId");

            migrationBuilder.CreateIndex(
                name: "locationId2",
                table: "shiftassignment",
                column: "locationId");

            migrationBuilder.CreateIndex(
                name: "shiftId",
                table: "shiftassignment",
                column: "shiftId");

            migrationBuilder.CreateIndex(
                name: "assignmentId",
                table: "shiftincident",
                column: "assignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_team_GuardId",
                table: "team",
                column: "GuardId");

            migrationBuilder.CreateIndex(
                name: "IX_users_RoleID",
                table: "users",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contract");

            migrationBuilder.DropTable(
                name: "refreshtoken");

            migrationBuilder.DropTable(
                name: "shiftincident");

            migrationBuilder.DropTable(
                name: "shiftassignment");

            migrationBuilder.DropTable(
                name: "securityshift");

            migrationBuilder.DropTable(
                name: "checkpoint");

            migrationBuilder.DropTable(
                name: "shifttype");

            migrationBuilder.DropTable(
                name: "team");

            migrationBuilder.DropTable(
                name: "location");

            migrationBuilder.DropTable(
                name: "securityguard");

            migrationBuilder.DropTable(
                name: "business");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
