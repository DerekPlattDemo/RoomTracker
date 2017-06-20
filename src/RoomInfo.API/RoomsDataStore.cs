using RoomInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomInfo.API
{
    public class RoomsDataStore
    {
        public static RoomsDataStore Current { get; } = new RoomsDataStore();
        public List<RoomDto> Rooms { get; set; }

        public RoomsDataStore()
        {
           //used for dummy data before setting up EF

        }
    }
}
