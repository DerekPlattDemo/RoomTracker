using RoomInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomInfo.API.Services
{
    public interface IRoomInfoRepository
    {
        bool RoomExists(int roomId);
        IEnumerable<Room> GetRooms();
        Room GetRoom(int roomId, bool includeItemsOfInterest);
        IEnumerable<ItemOfInterest> GetItemsOfInterestForRoom(int roomId);
        ItemOfInterest GetItemOfInterestForRoom(int roomId, int itemOfInterestId);
        void AddItemOfInterestForRoom(int roomId, ItemOfInterest itemOfInterest);
        void DeleteItemOfInterest(ItemOfInterest itemOfInterest);
        bool Save();
    }
}
