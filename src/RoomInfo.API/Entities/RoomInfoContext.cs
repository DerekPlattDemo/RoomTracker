using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomInfo.API.Entities
{
    public class RoomInfoContext : DbContext
    {
        public RoomInfoContext(DbContextOptions<RoomInfoContext> options)
           : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<ItemOfInterest> ItemsOfInterest { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("connectionstring");

        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
