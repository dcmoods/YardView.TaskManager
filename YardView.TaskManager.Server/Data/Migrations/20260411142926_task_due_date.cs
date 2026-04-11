using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YardView.TaskManager.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class task_due_date : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Tasks",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Tasks");
        }
    }
}
