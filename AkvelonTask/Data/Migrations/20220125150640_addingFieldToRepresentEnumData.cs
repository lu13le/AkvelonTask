using Microsoft.EntityFrameworkCore.Migrations;

namespace AkvelonTask.Migrations
{
    public partial class addingFieldToRepresentEnumData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskStatus",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectStatus",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskStatus",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ProjectStatus",
                table: "Projects");
        }
    }
}
