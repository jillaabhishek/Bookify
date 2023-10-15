using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_OutMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bookings_apartments_apartment_id",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_bookings_user_user_id",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_reviews_apartments_apartment_id",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "fk_reviews_bookings_booking_id",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "fk_reviews_user_user_id",
                table: "reviews");

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    occurred_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    processed_on_utc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_apartments_apartment_id1",
                table: "bookings",
                column: "apartment_id",
                principalTable: "apartments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_user_user_temp_id",
                table: "bookings",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_apartments_apartment_id1",
                table: "reviews",
                column: "apartment_id",
                principalTable: "apartments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_bookings_booking_id1",
                table: "reviews",
                column: "booking_id",
                principalTable: "bookings",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_user_user_temp_id",
                table: "reviews",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bookings_apartments_apartment_id1",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_bookings_user_user_temp_id",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "fk_reviews_apartments_apartment_id1",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "fk_reviews_bookings_booking_id1",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "fk_reviews_user_user_temp_id",
                table: "reviews");

            migrationBuilder.DropTable(
                name: "outbox_messages");

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_apartments_apartment_id",
                table: "bookings",
                column: "apartment_id",
                principalTable: "apartments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_bookings_user_user_id",
                table: "bookings",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_apartments_apartment_id",
                table: "reviews",
                column: "apartment_id",
                principalTable: "apartments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_bookings_booking_id",
                table: "reviews",
                column: "booking_id",
                principalTable: "bookings",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_reviews_user_user_id",
                table: "reviews",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
