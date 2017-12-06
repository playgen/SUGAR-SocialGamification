using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Server.EntityFramework.Migrations
{
    public partial class GenericRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserToGroupRelationshipRequests");

            migrationBuilder.DropTable(
                name: "UserToGroupRelationships");

            migrationBuilder.DropTable(
                name: "UserToUserRelationshipRequests");

            migrationBuilder.DropTable(
                name: "UserToUserRelationships");

            migrationBuilder.CreateTable(
                name: "RelationshipRequests",
                columns: table => new
                {
                    RequestorId = table.Column<int>(nullable: false),
                    AcceptorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelationshipRequests", x => new { x.RequestorId, x.AcceptorId });
                    table.ForeignKey(
                        name: "FK_RelationshipRequests_Actors_AcceptorId",
                        column: x => x.AcceptorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelationshipRequests_Actors_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relationships",
                columns: table => new
                {
                    RequestorId = table.Column<int>(nullable: false),
                    AcceptorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationships", x => new { x.RequestorId, x.AcceptorId });
                    table.ForeignKey(
                        name: "FK_Relationships_Actors_AcceptorId",
                        column: x => x.AcceptorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Relationships_Actors_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipRequests_AcceptorId",
                table: "RelationshipRequests",
                column: "AcceptorId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_AcceptorId",
                table: "Relationships",
                column: "AcceptorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelationshipRequests");

            migrationBuilder.DropTable(
                name: "Relationships");

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
    }
}
