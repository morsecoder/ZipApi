using Microsoft.EntityFrameworkCore;
using ZipApi.Models;

namespace ZipApi.Entities
{
    public class ZipDbContext : DbContext
    {
        public ZipDbContext(DbContextOptions<ZipDbContext> options)
            : base(options)
        {
        }

        public DbSet<LocationEntity> Locations { get; set; }
        public DbSet<CbsaEntity> Cbsas { get; set; }
        public DbSet<MsaEntity> Msas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}