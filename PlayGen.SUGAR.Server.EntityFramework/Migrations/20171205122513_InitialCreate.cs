using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Server.EntityFramework.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountSources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApiSecret = table.Column<string>(nullable: true),
                    AutoRegister = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    RequiresPassword = table.Column<bool>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    UsernamePattern = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActorData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActorId = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    EvaluationDataType = table.Column<int>(nullable: false),
                    GameId = table.Column<int>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Discriminator = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClaimScope = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Evaluations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActorType = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    GameId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leaderboards",
                columns: table => new
                {
                    Token = table.Column<string>(nullable: false),
                    GameId = table.Column<int>(nullable: false),
                    ActorType = table.Column<int>(nullable: false),
                    CriteriaScope = table.Column<int>(nullable: false),
                    EvaluationDataCategory = table.Column<int>(nullable: false),
                    EvaluationDataKey = table.Column<string>(nullable: true),
                    EvaluationDataType = table.Column<int>(nullable: false),
                    LeaderboardType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaderboards", x => new { x.Token, x.GameId });
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClaimScope = table.Column<int>(nullable: false),
                    Default = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SentEvaluationNotifications",
                columns: table => new
                {
                    GameId = table.Column<int>(nullable: false),
                    ActorId = table.Column<int>(nullable: false),
                    EvaluationId = table.Column<int>(nullable: false),
                    Progress = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SentEvaluationNotifications", x => new { x.GameId, x.ActorId, x.EvaluationId });
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountSourceId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountSources_AccountSourceId",
                        column: x => x.AccountSourceId,
                        principalTable: "AccountSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accounts_Actors_UserId",
                        column: x => x.UserId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToGroupRelationshipRequests",
                columns: table => new
                {
                    RequestorId = table.Column<int>(nullable: false),
                    AcceptorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToGroupRelationshipRequests", x => new { x.RequestorId, x.AcceptorId });
                    table.ForeignKey(
                        name: "FK_UserToGroupRelationshipRequests_Actors_AcceptorId",
                        column: x => x.AcceptorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserToGroupRelationshipRequests_Actors_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToGroupRelationships",
                columns: table => new
                {
                    RequestorId = table.Column<int>(nullable: false),
                    AcceptorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToGroupRelationships", x => new { x.RequestorId, x.AcceptorId });
                    table.ForeignKey(
                        name: "FK_UserToGroupRelationships_Actors_AcceptorId",
                        column: x => x.AcceptorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserToGroupRelationships_Actors_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToUserRelationshipRequests",
                columns: table => new
                {
                    RequestorId = table.Column<int>(nullable: false),
                    AcceptorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToUserRelationshipRequests", x => new { x.RequestorId, x.AcceptorId });
                    table.ForeignKey(
                        name: "FK_UserToUserRelationshipRequests_Actors_AcceptorId",
                        column: x => x.AcceptorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserToUserRelationshipRequests_Actors_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToUserRelationships",
                columns: table => new
                {
                    RequestorId = table.Column<int>(nullable: false),
                    AcceptorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToUserRelationships", x => new { x.RequestorId, x.AcceptorId });
                    table.ForeignKey(
                        name: "FK_UserToUserRelationships_Actors_AcceptorId",
                        column: x => x.AcceptorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserToUserRelationships_Actors_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActorClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActorId = table.Column<int>(nullable: false),
                    ClaimId = table.Column<int>(nullable: false),
                    EntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActorClaims_Actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorClaims_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationCriterias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ComparisonType = table.Column<int>(nullable: false),
                    CriteriaQueryType = table.Column<int>(nullable: false),
                    EvaluationDataCategory = table.Column<int>(nullable: false),
                    EvaluationDataKey = table.Column<string>(maxLength: 64, nullable: false),
                    EvaluationDataType = table.Column<int>(nullable: false),
                    EvaluationId = table.Column<int>(nullable: true),
                    Scope = table.Column<int>(nullable: false),
                    Value = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationCriterias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationCriterias_Evaluations_EvaluationId",
                        column: x => x.EvaluationId,
                        principalTable: "Evaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EvaluationDataCategory = table.Column<int>(nullable: false),
                    EvaluationDataKey = table.Column<string>(maxLength: 64, nullable: false),
                    EvaluationDataType = table.Column<int>(nullable: false),
                    EvaluationId = table.Column<int>(nullable: true),
                    Value = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rewards_Evaluations_EvaluationId",
                        column: x => x.EvaluationId,
                        principalTable: "Evaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    Ended = table.Column<DateTime>(nullable: true),
                    GameId = table.Column<int>(nullable: false),
                    Started = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Actors_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActorRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActorId = table.Column<int>(nullable: false),
                    EntityId = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActorRoles_Actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    ClaimId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => new { x.RoleId, x.ClaimId });
                    table.ForeignKey(
                        name: "FK_RoleClaims_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActorId = table.Column<int>(nullable: true),
                    Category = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    EvaluationDataType = table.Column<int>(nullable: false),
                    GameId = table.Column<int>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    MatchId = table.Column<int>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationData_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountSourceId",
                table: "Accounts",
                column: "AccountSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Name_AccountSourceId",
                table: "Accounts",
                columns: new[] { "Name", "AccountSourceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActorClaims_ClaimId",
                table: "ActorClaims",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorClaims_ActorId_EntityId_ClaimId",
                table: "ActorClaims",
                columns: new[] { "ActorId", "EntityId", "ClaimId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActorData_Key_GameId_ActorId_EvaluationDataType",
                table: "ActorData",
                columns: new[] { "Key", "GameId", "ActorId", "EvaluationDataType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActorRoles_RoleId",
                table: "ActorRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorRoles_ActorId_EntityId_RoleId",
                table: "ActorRoles",
                columns: new[] { "ActorId", "EntityId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actors_Name",
                table: "Actors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationCriterias_EvaluationId",
                table: "EvaluationCriterias",
                column: "EvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationData_MatchId",
                table: "EvaluationData",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationData_Game_Category_Creator_Type",
                table: "EvaluationData",
                columns: new[] { "GameId", "Category", "ActorId", "EvaluationDataType" });

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationData_Game_Category_Match_Type",
                table: "EvaluationData",
                columns: new[] { "GameId", "Category", "MatchId", "EvaluationDataType" });

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationData_Key_Game_Category_Creator_Type",
                table: "EvaluationData",
                columns: new[] { "Key", "GameId", "Category", "ActorId", "EvaluationDataType" });

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_Token_GameId_ActorType",
                table: "Evaluations",
                columns: new[] { "Token", "GameId", "ActorType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_Name",
                table: "Games",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_CreatorId",
                table: "Matches",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_GameId",
                table: "Matches",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_EvaluationId",
                table: "Rewards",
                column: "EvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_ClaimId",
                table: "RoleClaims",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToGroupRelationshipRequests_AcceptorId",
                table: "UserToGroupRelationshipRequests",
                column: "AcceptorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToGroupRelationships_AcceptorId",
                table: "UserToGroupRelationships",
                column: "AcceptorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToUserRelationshipRequests_AcceptorId",
                table: "UserToUserRelationshipRequests",
                column: "AcceptorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToUserRelationships_AcceptorId",
                table: "UserToUserRelationships",
                column: "AcceptorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "ActorClaims");

            migrationBuilder.DropTable(
                name: "ActorData");

            migrationBuilder.DropTable(
                name: "ActorRoles");

            migrationBuilder.DropTable(
                name: "EvaluationCriterias");

            migrationBuilder.DropTable(
                name: "EvaluationData");

            migrationBuilder.DropTable(
                name: "Leaderboards");

            migrationBuilder.DropTable(
                name: "Rewards");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SentEvaluationNotifications");

            migrationBuilder.DropTable(
                name: "UserToGroupRelationshipRequests");

            migrationBuilder.DropTable(
                name: "UserToGroupRelationships");

            migrationBuilder.DropTable(
                name: "UserToUserRelationshipRequests");

            migrationBuilder.DropTable(
                name: "UserToUserRelationships");

            migrationBuilder.DropTable(
                name: "AccountSources");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Evaluations");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
