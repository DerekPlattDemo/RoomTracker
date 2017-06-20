using AutoMapper;
using RoomInfo.API.Models;
using RoomInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomInfo.API.Controllers
{
    [Route("api/rooms")]
    public class RoomsController : Controller
    {
        private IRoomInfoRepository _roomInfoRepository;

        public RoomsController(IRoomInfoRepository roomInfoRepository)
        {
            _roomInfoRepository = roomInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetRooms()
        {
            var roomEntities = _roomInfoRepository.GetRooms();
            var results = Mapper.Map<IEnumerable<RoomWithoutItemsOfInterestDto>>(roomEntities); 

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetRoom(int id, bool includeItemsOfInterest = false)
        {
            var room = _roomInfoRepository.GetRoom(id, includeItemsOfInterest);

            if (room == null)
            {
                return NotFound();
            }

            if (includeItemsOfInterest)
            {
                var roomResult = Mapper.Map<RoomDto>(room); 
                return Ok(roomResult);
            }

            var roomWithoutItemsOfInterestResult = Mapper.Map<RoomWithoutItemsOfInterestDto>(room);
            return Ok(roomWithoutItemsOfInterestResult);
        }
    }
}
