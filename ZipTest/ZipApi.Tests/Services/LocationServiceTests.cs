using System;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ZipApi.Entities;
using ZipApi.Services;
using System.Linq;
using System.Collections.Generic;
using ZipApi.Models;
namespace ZipApi.Tests
{
    public class LocationServiceTests
    {
        [Fact]
        public void GetLocationShouldPullCorrectData()
        {
            var options = new DbContextOptionsBuilder<ZipDbContext>()
                .UseInMemoryDatabase(databaseName: "my_db")
                .Options;

            string zip = "90403";
            string name = "LA";

            // Run the test against one instance of the context
            using (var context = new ZipDbContext(options))
            {
                context.Cbsas.Add(new CbsaEntity
                {
                    Zip = zip,
                    Cbsa = "12345"
                });

                context.Msas.Add(new MsaEntity
                {
                    Cbsa = "12345",
                    Mdiv = "",
                    Lsad = "Metropolitan Statistical Area",
                    PopEstimate2014 = "100",
                    PopEstimate2015 = "200",
                    Name = name
                });

                context.SaveChanges();

                LocationService service = new LocationService(context);

                List<Location> locationData = service.GetLocationData(zip);

                Assert.True(locationData.Any(x => x.Msa == name));
            }
        }

        [Fact]
        public void GetLocationShouldPulNo()
        {
            var options = new DbContextOptionsBuilder<ZipDbContext>()
                .UseInMemoryDatabase(databaseName: "my_db")
                .Options;
               
            string zip = "90403";
            string name = "LA";
            string alt = "54321";

            // Run the test against one instance of the context
            using (var context = new ZipDbContext(options))
            {
                context.Cbsas.Add(new CbsaEntity
                {
                    Zip = zip,
                    Cbsa = "12345"
                });

                context.Msas.Add(new MsaEntity
                {
                    Cbsa = alt,
                    Mdiv = "12345",
                    Lsad = "Metropolitan Statistical Area",
                    PopEstimate2014 = "100",
                    PopEstimate2015 = "200",
                    Name = name
                });

                context.SaveChanges();

                LocationService service = new LocationService(context);

                List<Location> locationData = service.GetLocationData(zip);

                Assert.True(locationData.Any(x => x.Cbsa == alt));
            }
        }
    }
}
