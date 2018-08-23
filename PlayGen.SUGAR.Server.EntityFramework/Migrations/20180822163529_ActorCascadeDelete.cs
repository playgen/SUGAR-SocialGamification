using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Server.EntityFramework.Migrations
{
    public partial class ActorCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ActorId",
                table: "EvaluationData",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationData_ActorId",
                table: "EvaluationData",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorData_ActorId",
                table: "ActorData",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorData_GameId",
                table: "ActorData",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorData_Actors_ActorId",
                table: "ActorData",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActorData_Games_GameId",
                table: "ActorData",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationData_Actors_ActorId",
                table: "EvaluationData",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorData_Actors_ActorId",
                table: "ActorData");

            migrationBuilder.DropForeignKey(
                name: "FK_ActorData_Games_GameId",
                table: "ActorData");

            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationData_Actors_ActorId",
                table: "EvaluationData");

            migrationBuilder.DropIndex(
                name: "IX_EvaluationData_ActorId",
                table: "EvaluationData");

            migrationBuilder.DropIndex(
                name: "IX_ActorData_ActorId",
                table: "ActorData");

            migrationBuilder.DropIndex(
                name: "IX_ActorData_GameId",
                table: "ActorData");

            migrationBuilder.AlterColumn<int>(
                name: "ActorId",
                table: "EvaluationData",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
