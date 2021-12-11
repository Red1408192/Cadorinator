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
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "anteo.spaziocinema.18tickets.it/", 1 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "citylife.spaziocinema.18tickets.it/", 1 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "capitol.spaziocinema.18tickets.it/", 1 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "ariosto.spaziocinema.18tickets.it/", 1 });

            //Roma
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 1, "eurcine.ccroma.18tickets.it/", 2 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 1, "giuliocesare.ccroma.18tickets.it/", 2 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 1, "king.ccroma.18tickets.it/", 2 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 1, "nuovoolimpia.ccroma.18tickets.it/", 2 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 1, "quattrofontane.ccroma.18tickets.it/", 2 });

            //Torino
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "romano.cctorino.18tickets.it/", 3 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "eliseo.cctorino.18tickets.it/", 3 });

            //Bologna
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "lumiere.cinetecabologna.18tickets.it/", 4 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "puccini.cinetecabologna.18tickets.it/", 4 });

            //Firenze
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "spazio.spazioalfieri.18tickets.it/", 5 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "uffizi.spazioalfieri.18tickets.it/", 5 });

            //Genova
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "america.ccgenova.18tickets.it/", 6 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "palazzoducale.ccgenova.18tickets.it/", 6 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "ariston.ccgenova.18tickets.it/", 6 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "city.ccgenova.18tickets.it/", 6 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "corallo.ccgenova.18tickets.it/", 6 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "carignano.ccgenova.18tickets.it/", 6 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "odeon.ccgenova.18tickets.it/", 6 });
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "sivori.ccgenova.18tickets.it/", 6 });

            //Napoli
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "filangieri.cinemadinapoli.18tickets.it/", 7 });

            //Bari
            migrationBuilder.InsertData("Provider", new[] { "ProviderSource", "ProviderDomain", "CityId" }, new object[] { 2, "multicinemagalleria.18tickets.it/", 8 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
