using System.Collections.Generic;
using ZipApi.Entities;
using ZipApi.Models;
using System.Linq;
using ZipApi.Interfaces;
using System;

namespace ZipApi.Services
{
    public class LocationService : ILocationService
    {
        private readonly ZipDbContext _context;

        private readonly string StatisticalArea = "Metropolitan Statistical Area";
        private readonly string Missing = "N/A";

        public LocationService(ZipDbContext context)
        {
            _context = context;
        }

        public List<Location> GetLocations()
        {
            return _context.Cbsas
                .Select(x => new Location
                {
                    Zip = x.Zip,
                    Cbsa = x.Cbsa
                })
                .ToList();
        }

        public List<Location> GetLocationData(string zip)
        {
            if(!ValidateZip(zip))
            {
                return null;
            }

            //Get from zip
            var cbsas = _context.Cbsas
                .Where(x => x.Zip == zip)
                .Select(l => l.Cbsa)
                .ToList();
                
            var alternateCbsas = _context.Msas
                .Where(x => cbsas.Contains(x.Mdiv))
                .Select(x => new
                {
                    x.Cbsa,
                    Mdiv = x.Mdiv
                })
                .ToList();

            var correctCbsas = new List<string>();
            foreach(var cbsa in cbsas)
            {
                string alternative = alternateCbsas.Where(x => x.Mdiv == cbsa).Select(x => x.Cbsa).FirstOrDefault();
                if (!string.IsNullOrEmpty(alternative))
                {
                    correctCbsas.Add(alternative);
                }
                else
                {
                    correctCbsas.Add(cbsa);
                }
            }

            var locationData = _context.Msas
                .Where(x => correctCbsas.Contains(x.Cbsa) && x.Lsad == StatisticalArea)
                .Select(x => new Location
                {
                    Zip = zip,
                    Cbsa = x.Cbsa,
                    Msa = x.Name,
                    Pop2014 = x.PopEstimate2014,
                    Pop2015 = x.PopEstimate2015
                })
                .ToList();

            if(locationData != null && !locationData.Any())
            {
                locationData.Add(new Location
                {
                    Zip = zip,
                    Cbsa = correctCbsas.FirstOrDefault(),
                    Msa = Missing,
                    Pop2014 = Missing,
                    Pop2015 = Missing
                });
            }

            return locationData;

        }

        private bool ValidateZip(string zip)
        {
            bool isParsed = int.TryParse(zip, out int zipNumber);
            return isParsed && zipNumber > 0 && zipNumber <= 99999;
        }
    }
}
