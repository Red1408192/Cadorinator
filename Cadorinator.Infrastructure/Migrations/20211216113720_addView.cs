using Microsoft.EntityFrameworkCore.Migrations;

namespace Cadorinator.Infrastructure.Migrations
{
    public partial class addView : Migration
    {
        private readonly string sqlscriptUp = @"
CREATE VIEW LatestSampleView AS
WITH partition as (
SELECT
     F.FilmName AS Film
    , P.ProviderDomain AS Provider
    , C.CityName AS City
    , T.TheaterName AS Theater
    , Date(PS.ProjectionTimestamp, 'localtime')   AS Date
     , Time(PS.ProjectionTimestamp, 'localtime')   AS Time
      , Sp.TotalSeats AS 'Total'
    , Sp.BoughtSeats AS 'Bought'
    , Sp.LockedSeats AS 'Locked'
    , Sp.QuarantinedSeats AS 'Quarantined'
    , Sp.Eta AS eta
    ,ROW_NUMBER() OVER(PARTITION BY F.FilmName, P.ProviderDomain, C.Cityname, T.TheaterName, Date(PS.ProjectionTimestamp, 'localtime'), Time(PS.ProjectionTimestamp, 'localtime') ORDER BY Sp.SampleTimestamp DESC) AS row_number
FROM ProjectionsSchedule PS
JOIN Sample SP on PS.ProjectionsScheduleId = SP.ProjectionsScheduleId
JOIN Provider P ON P.ProviderId = PS.ProviderId
JOIN Film F ON F.FilmId = PS.FilmId
JOIN City c ON c.CityId = P.CityId
JOIN Theater T ON T.TheaterId = PS.ThreaterId
)
SELECT
     Film
    , Provider
    , City
    , Theater
    , Date
    , Time
    , Total
    , Bought
    , Locked
    , Quarantined
    ,eta as 'Sample Eta'
FROM partition
WHERE row_number = 1
ORDER BY DATE, TIME";

        private readonly string sqlscriptDown = "DROP VIEW LatestSampleView";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(sqlscriptUp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(sqlscriptDown);
        }
    }
}
