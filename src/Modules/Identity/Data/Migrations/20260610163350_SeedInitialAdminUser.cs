using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerTracker.Identity.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM ""Users"" WHERE ""Email"" = 'admin@careertracker.local') THEN
                        INSERT INTO ""Users"" (""Email"", ""PasswordHash"", ""Role"", ""IsActive"", ""CreatedAt"")
                        VALUES (
                            'admin@careertracker.local',
                            'AQAAAAIAAYagAAAAEOPvL5qV8Y3K7lZ9wX2bN4mP5qR6sT7uV8wX9yZ0aB1cD2eF3gH4iJ5kL6mN7oP8qR9s==',
                            'Admin',
                            TRUE,
                            NOW()
                        );

                        INSERT INTO ""UserProfiles"" (""UserId"", ""FirstName"", ""LastName"", ""Attributes"", ""CreatedAt"")
                        SELECT ""Id"", 'Системный', 'Администратор', '{}'::jsonb, NOW()
                        FROM ""Users""
                        WHERE ""Email"" = 'admin@careertracker.local';
                    END IF;
                END $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM ""UserProfiles"" 
                WHERE ""UserId"" IN (SELECT ""Id"" FROM ""Users"" WHERE ""Email"" = 'admin@careertracker.local');
                
                DELETE FROM ""Users"" 
                WHERE ""Email"" = 'admin@careertracker.local';
            ");
        }
    }
}
