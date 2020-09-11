using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CmApp.Contracts.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Makes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Makes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Sex = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    BornDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    Blocked = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    CarMakesEntityID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Models_Makes_CarMakesEntityID",
                        column: x => x.CarMakesEntityID,
                        principalTable: "Makes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResets",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(nullable: true),
                    ValidUntil = table.Column<DateTime>(nullable: false),
                    UserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResets", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PasswordResets_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vin = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ManufactureDate = table.Column<DateTime>(nullable: false),
                    Series = table.Column<string>(nullable: true),
                    BodyType = table.Column<string>(nullable: true),
                    Steering = table.Column<string>(nullable: true),
                    Engine = table.Column<string>(nullable: true),
                    Displacement = table.Column<double>(nullable: false),
                    Power = table.Column<string>(nullable: true),
                    Drive = table.Column<string>(nullable: true),
                    Transmission = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    Interior = table.Column<string>(nullable: true),
                    MakeID = table.Column<int>(nullable: true),
                    ModelID = table.Column<int>(nullable: true),
                    UserEntityID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cars_Makes_MakeID",
                        column: x => x.MakeID,
                        principalTable: "Makes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cars_Models_ModelID",
                        column: x => x.ModelID,
                        principalTable: "Models",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cars_Users_UserEntityID",
                        column: x => x.UserEntityID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CarEntityID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Equipment_Cars_CarEntityID",
                        column: x => x.CarEntityID,
                        principalTable: "Cars",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Repairs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Car = table.Column<int>(nullable: false),
                    Total = table.Column<double>(nullable: false),
                    CarEntityID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repairs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Repairs_Cars_CarEntityID",
                        column: x => x.CarEntityID,
                        principalTable: "Cars",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shippings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    Customs = table.Column<double>(nullable: false),
                    AuctionFee = table.Column<double>(nullable: false),
                    TransferFee = table.Column<double>(nullable: false),
                    TransportationFee = table.Column<double>(nullable: false),
                    BaseCurrency = table.Column<string>(nullable: true),
                    CustomsCurrency = table.Column<string>(nullable: true),
                    AuctionFeeCurrency = table.Column<string>(nullable: true),
                    TransferFeeCurrency = table.Column<string>(nullable: true),
                    TransportationFeeCurrency = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shippings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Shippings_Cars_ID",
                        column: x => x.ID,
                        principalTable: "Cars",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Summaries",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    BoughtPrice = table.Column<double>(nullable: false),
                    SoldPrice = table.Column<double>(nullable: false),
                    SoldDate = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    SoldWithin = table.Column<string>(nullable: true),
                    Sold = table.Column<bool>(nullable: false),
                    Total = table.Column<double>(nullable: false),
                    BaseCurrency = table.Column<string>(nullable: true),
                    SelectedCurrency = table.Column<string>(nullable: true),
                    Profit = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summaries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Summaries_Cars_ID",
                        column: x => x.ID,
                        principalTable: "Cars",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trackings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    Vin = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Make = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    ShowImages = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    DateReceived = table.Column<DateTime>(nullable: false),
                    DateOrdered = table.Column<DateTime>(nullable: false),
                    Branch = table.Column<string>(nullable: true),
                    ShippingLine = table.Column<string>(nullable: true),
                    ContainerNumber = table.Column<string>(nullable: true),
                    BookingNumber = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    FinalPort = table.Column<string>(nullable: true),
                    DatePickedUp = table.Column<DateTime>(nullable: false),
                    Color = table.Column<string>(nullable: true),
                    Damage = table.Column<string>(nullable: true),
                    Condition = table.Column<string>(nullable: true),
                    Keys = table.Column<string>(nullable: true),
                    Running = table.Column<string>(nullable: true),
                    Wheels = table.Column<string>(nullable: true),
                    AirBag = table.Column<string>(nullable: true),
                    Radio = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trackings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Trackings_Cars_ID",
                        column: x => x.ID,
                        principalTable: "Cars",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Urls",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(nullable: true),
                    CarEntityID = table.Column<int>(nullable: true),
                    TrackingEntityID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urls", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Urls_Cars_CarEntityID",
                        column: x => x.CarEntityID,
                        principalTable: "Cars",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Urls_Trackings_TrackingEntityID",
                        column: x => x.TrackingEntityID,
                        principalTable: "Trackings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_MakeID",
                table: "Cars",
                column: "MakeID");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ModelID",
                table: "Cars",
                column: "ModelID");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_UserEntityID",
                table: "Cars",
                column: "UserEntityID");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_CarEntityID",
                table: "Equipment",
                column: "CarEntityID");

            migrationBuilder.CreateIndex(
                name: "IX_Models_CarMakesEntityID",
                table: "Models",
                column: "CarMakesEntityID");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResets_UserID",
                table: "PasswordResets",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_CarEntityID",
                table: "Repairs",
                column: "CarEntityID");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_CarEntityID",
                table: "Urls",
                column: "CarEntityID");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_TrackingEntityID",
                table: "Urls",
                column: "TrackingEntityID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "PasswordResets");

            migrationBuilder.DropTable(
                name: "Repairs");

            migrationBuilder.DropTable(
                name: "Shippings");

            migrationBuilder.DropTable(
                name: "Summaries");

            migrationBuilder.DropTable(
                name: "Urls");

            migrationBuilder.DropTable(
                name: "Trackings");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Makes");
        }
    }
}
