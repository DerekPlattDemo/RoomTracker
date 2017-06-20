using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoomInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace RoomInfo.API.Services
{
    public class RoomInfoRepository : IRoomInfoRepository
    {
        private RoomInfoContext _context;
        public RoomInfoRepository(RoomInfoContext context)
        {
            _context = context;
        }

        public void AddItemOfInterestForRoom(int roomId, ItemOfInterest itemOfInterest)
        {
            var room = GetRoom(roomId, false);
            room.ItemsOfInterest.Add(itemOfInterest);
        }

        public bool RoomExists(int roomId)
        {
            return _context.Rooms.Any(c => c.Id == roomId);
        }

        public IEnumerable<Room> GetRooms()
        {
            return _context.Rooms.OrderBy(c => c.Name).ToList();
        }

        public Room GetRoom(int roomId, bool includeItemsOfInterest)
        {
            if (includeItemsOfInterest)
            {
                return _context.Rooms.Include(c => c.ItemsOfInterest)
                    .Where(c => c.Id == roomId).FirstOrDefault();
            }

            return _context.Rooms.Where(c => c.Id == roomId).FirstOrDefault();
        }

        public ItemOfInterest GetItemOfInterestForRoom(int roomId, int itemOfInterestId)
        {
            return _context.ItemsOfInterest
               .Where(p => p.RoomId == roomId && p.Id == itemOfInterestId).FirstOrDefault();
        }

        public IEnumerable<ItemOfInterest> GetItemsOfInterestForRoom(int roomId)
        {
            return _context.ItemsOfInterest
                           .Where(p => p.RoomId == roomId).ToList();
        }

        public void DeleteItemOfInterest(ItemOfInterest itemOfInterest)
        {
            _context.ItemsOfInterest.Remove(itemOfInterest);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
