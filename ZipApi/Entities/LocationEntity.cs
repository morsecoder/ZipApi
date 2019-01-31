using System;
using System.Collections.Generic;

namespace ZipApi.Entities
{
    public class LocationEntity
    {
        public long Id { get; set; }
        public int Zip { get; set; }
        public int Cbsa { get; set; }
    }
}