using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomInfo.API.Models
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int NumberOfItemsOfInterest { get
            {
                return ItemsOfInterest.Count;
            }
        }

        public ICollection<ItemOfInterestDto> ItemsOfInterest { get; set; }
        = new List<ItemOfInterestDto>();
    }
}
