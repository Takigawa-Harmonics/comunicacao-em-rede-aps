using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComunicacaoEmRedesApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class TokenEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "session_tokens",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    hash_token = table.Column<string>(type: "TEXT", maxLength: 70, nullable: false),
                    expires_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    is_revoked = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session_tokens", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_session_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "session_tokens");
        }
    }
}
