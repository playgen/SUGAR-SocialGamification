using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Server.EntityFramework.Migrations
{
    public partial class MemberCountAndAllianceCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationCriterias_Evaluations_EvaluationId",
                table: "EvaluationCriterias");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_Evaluations_EvaluationId",
                table: "Rewards");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Rewards",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<int>(
                name: "EvaluationId",
                table: "Rewards",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EvaluationDataKey",
                table: "Rewards",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "EvaluationCriterias",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<int>(
                name: "EvaluationId",
                table: "EvaluationCriterias",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EvaluationDataKey",
                table: "EvaluationCriterias",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64);

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationCriterias_Evaluations_EvaluationId",
                table: "EvaluationCriterias",
                column: "EvaluationId",
                principalTable: "Evaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_Evaluations_EvaluationId",
                table: "Rewards",
                column: "EvaluationId",
                principalTable: "Evaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationCriterias_Evaluations_EvaluationId",
                table: "EvaluationCriterias");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_Evaluations_EvaluationId",
                table: "Rewards");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Rewards",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EvaluationId",
                table: "Rewards",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "EvaluationDataKey",
                table: "Rewards",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "EvaluationCriterias",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EvaluationId",
                table: "EvaluationCriterias",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "EvaluationDataKey",
                table: "EvaluationCriterias",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationCriterias_Evaluations_EvaluationId",
                table: "EvaluationCriterias",
                column: "EvaluationId",
                principalTable: "Evaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_Evaluations_EvaluationId",
                table: "Rewards",
                column: "EvaluationId",
                principalTable: "Evaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
