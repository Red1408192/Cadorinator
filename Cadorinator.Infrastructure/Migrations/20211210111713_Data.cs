using Microsoft.EntityFrameworkCore.Migrations;

namespace Cadorinator.Infrastructure.Migrations
{
    public partial class Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Cities
            migrationBuilder.InsertData("City", "CityName", "Milano");
            migrationBuilder.InsertData("City", "CityName", "Roma");
            migrationBuilder.InsertData("City", "CityName", "Torino");
            migrationBuilder.InsertData("City", "CityName", "Bologna");
            migrationBuilder.InsertData("City", "CityName", "Firenze");
            migrationBuilder.InsertData("City", "CityName", "Genova");
            migrationBuilder.InsertData("City", "CityName", "Napoli");
            migrationBuilder.InsertData("City", "CityName", "Bari");

            //ProviderSource
            migrationBuilder.InsertData("ProviderSource", "ProviderSourceName", "18TicketsV1");
            migrationBuilder.InsertData("ProviderSource", "ProviderSourceName", "18TicketsV2");

            ////Providers
            //Milan
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "anteo.spaziocinema.18tickets.it", 1, "Anteo" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "citylife.spaziocinema.18tickets.it", 1, "Citylife" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "capitol.spaziocinema.18tickets.it", 1, "Capitol" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "ariosto.spaziocinema.18tickets.it", 1, "Ariosto" });

            //Roma
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 1, "eurcine.ccroma.18tickets.it", 2, "Eurcine" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 1, "giuliocesare.ccroma.18tickets.it", 2, "Giulio Cesare" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 1, "king.ccroma.18tickets.it", 2, "King" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 1, "nuovoolimpia.ccroma.18tickets.it", 2, "Nuovo Olimpia" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 1, "quattrofontane.ccroma.18tickets.it", 2, "Quattro Fondane" });

            //Torino
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "romano.cctorino.18tickets.it", 3, "Romano" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "eliseo.cctorino.18tickets.it", 3, "Eliseo" });

            //Bologna
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "lumiere.cinetecabologna.18tickets.it", 4, "Lumiere" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "puccini.cinetecabologna.18tickets.it", 4, "Area Puccini" });

            //Firenze
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "spazio.spazioalfieri.18tickets.it", 5, "Spazio Alfieri" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "uffizi.spazioalfieri.18tickets.it", 5, "Uffizi" });

            //Genova
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "america.ccgenova.18tickets.it", 6, "America" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "palazzoducale.ccgenova.18tickets.it", 6, "Palazzo Ducale" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "ariston.ccgenova.18tickets.it", 6, "Ariston" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "city.ccgenova.18tickets.it", 6, "City" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "corallo.ccgenova.18tickets.it", 6, "Corallo" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "carignano.ccgenova.18tickets.it", 6, "Carigano" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "odeon.ccgenova.18tickets.it", 6, "Odeon" });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "sivori.ccgenova.18tickets.it", 6, "Sivori" });

            //Napoli
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "filangieri.cinemadinapoli.18tickets.it", 7, "Filangieri" });

            //Bari
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId", "ProviderName" }, new object[] { 2, "multicinemagalleria.18tickets.it", 8, "Galleria" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
