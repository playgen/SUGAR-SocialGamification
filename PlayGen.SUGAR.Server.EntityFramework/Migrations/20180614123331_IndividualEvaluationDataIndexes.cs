using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Server.EntityFramework.Migrations
{
    public partial class IndividualEvaluationDataIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EvaluationData_Game_Category_Creator_Type",
                table: "EvaluationData");

            migrationBuilder.DropIndex(
                name: "IX_EvaluationData_Game_Category_Match_Type",
                table: "EvaluationData");

            migrationBuilder.DropIndex(
                name: "IX_EvaluationData_Key_Game_Category_Creator_Type",
                table: "EvaluationData");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationData_ActorId",
                table: "EvaluationData",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationData_Category",
                table: "EvaluationData",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationData_Key",
                table: "EvaluationData",
                column: "Key");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EvaluationData_ActorId",
                table: "EvaluationData");

            migrationBuilder.DropIndex(
                name: "IX_EvaluationData_Category",
                table: "EvaluationData");

            migrationBuilder.DropIndex(
                name: "IX_EvaluationData_Key",
                table: "EvaluationData");

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
        }
    }
}
