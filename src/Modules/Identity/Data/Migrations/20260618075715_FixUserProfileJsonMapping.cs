using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerTracker.Identity.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixUserProfileJsonMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Attributes",
                table: "UserProfiles",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(Dictionary<string, object>),
                oldType: "jsonb",
                oldDefaultValueSql: "'{}'::jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Dictionary<string, object>>(
                name: "Attributes",
                table: "UserProfiles",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::jsonb",
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
