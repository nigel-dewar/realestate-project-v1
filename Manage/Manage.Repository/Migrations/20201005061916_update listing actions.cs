﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Manage.Repository.Migrations
{
    public partial class updatelistingactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "ListingActions");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "ListingActions",
                newName: "DateFulfilled");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Created",
                table: "ListingActions",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "FulfillmentId",
                table: "ListingActions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "ListingActions");

            migrationBuilder.DropColumn(
                name: "FulfillmentId",
                table: "ListingActions");

            migrationBuilder.RenameColumn(
                name: "DateFulfilled",
                table: "ListingActions",
                newName: "DateTime");

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "ListingActions",
                nullable: false,
                defaultValue: false);
        }
    }
}
