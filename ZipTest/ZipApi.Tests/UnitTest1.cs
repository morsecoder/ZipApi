////using System;
////using Xunit;

////namespace ZipApi.Tests
////{
////    public class UnitTest1
////    {
////        [Fact]
////        public void Test1()
////        {

////        }
////    }
////}



//using System;
//using Microsoft.EntityFrameworkCore;
//using Xunit;
//using ZipApi.Entities;
//using ZipApi.Services;
//using System.Linq;
//using System.Collections.Generic;
//using ZipApi.Models;
//namespace ZipApi.Tests
//{
//    public class LocationServiceTests
//    {
//        [Fact]
//        public void GetLocationShouldPullCorrectData()
//        {
//            var options = new DbContextOptionsBuilder<ZipDbContext>()
//                .UseInMemoryDatabase(databaseName: "my_db")
//                .Options;

//            //        Zip: 90266
//            //CBSA: 31084
//            //MSA: Los Angeles-Long Beach - Anaheim, CA
//            //  Pop2015: 13,340,068
//            //Pop2014: 13,254,397


//            //public long Id { get; set; }
//            //public string Cbsa { get; set; }
//            //public string Mdiv { get; set; }
//            //public string Lsad { get; set; }
//            //public string PopEstimate2014 { get; set; }
//            //public string PopEstimate2015 { get; set; }
//            //public string Name { get; set; }

//            string zip = "90403";
//            string name = "LA";

//            // Run the test against one instance of the context
//            using (var context = new ZipDbContext(options))
//            {
//                context.Cbsas.Add(new CbsaEntity
//                {
//                    Zip = zip,
//                    Cbsa = "12345"
//                });

//                context.Msas.Add(new MsaEntity
//                {
//                    Cbsa = "12345",
//                    Mdiv = "",
//                    Lsad = "Metropolitan Statistical Area",
//                    PopEstimate2014 = "100",
//                    PopEstimate2015 = "200",
//                    Name = name
//                });

//                context.SaveChanges();

//                LocationService service = new LocationService(context);

//                List<Location> locationData = service.GetLocationData(zip);

//                Assert.True(locationData.Any(x => x.Msa == name));

//            }
//        }
//    }
//}
