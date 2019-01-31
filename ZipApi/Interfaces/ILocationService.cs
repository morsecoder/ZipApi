using System;
using System.Collections.Generic;
using ZipApi.Entities;
using ZipApi.Models;

namespace ZipApi.Interfaces
{
    public interface ILocationService
    {
        List<Location> GetLocations();
        List<Location> GetLocationData(string zip);
    }
}
