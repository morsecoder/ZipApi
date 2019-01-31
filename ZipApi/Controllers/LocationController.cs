using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ZipApi.Entities;
using ZipApi.Interfaces;
using ZipApi.Models;

namespace ZipApi.Controllers
{
    [Route("api/locations")]
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly IDataLoadingService _dataLoadingService;

        public LocationController(ILocationService locationService, IDataLoadingService dataLoadingService)
        {
            _locationService = locationService;
            _dataLoadingService = dataLoadingService;
        }

        [HttpGet]
        public List<Location> GetLocations()
        {
            return _locationService.GetLocations();
        }

        [HttpGet("{zip}")]
        public List<Location> GetLocationData(string zip)
        {
            return _locationService.GetLocationData(zip);
        }

        [HttpPost("action/load-data")]
        public bool LoadData()
        {
            return _dataLoadingService.LoadData();
        }

    }
}
