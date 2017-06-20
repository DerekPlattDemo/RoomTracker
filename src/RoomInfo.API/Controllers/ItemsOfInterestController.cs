using AutoMapper;
using RoomInfo.API.Models;
using RoomInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomInfo.API.Controllers
{
    [Route("api/rooms")]
    public class ItemsOfInterestController : Controller
    {
        private ILogger<ItemsOfInterestController> _logger;
        private IRoomInfoRepository _roomInfoRepository;


        public ItemsOfInterestController(ILogger<ItemsOfInterestController> logger,
            IRoomInfoRepository roomInfoRepository)
        {
            _logger = logger;
            _roomInfoRepository = roomInfoRepository;
        }

        [HttpGet("{roomId}/itemsofinterest")]
        public IActionResult GetItemsOfInterest(int roomId)
        {
            try
            {
                if (!_roomInfoRepository.RoomExists(roomId))
                {
                    _logger.LogInformation($"Room with id {roomId} wasn't found when accessing items of interest.");
                    return NotFound();
                }

                var itemsOfInterestForRoom = _roomInfoRepository.GetItemsOfInterestForRoom(roomId);
                var itemsOfInterestForRoomResults =
                                   Mapper.Map<IEnumerable<ItemOfInterestDto>>(itemsOfInterestForRoom);

                return Ok(itemsOfInterestForRoomResults);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting items of interest for room with id {roomId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{roomId}/itemsofinterest/{id}", Name = "GetItemOfInterest")]
        public IActionResult GetItemOfInterest(int roomId, int id)
        {
            if (!_roomInfoRepository.RoomExists(roomId))
            {
                return NotFound();
            }

            var itemOfInterest = _roomInfoRepository.GetItemOfInterestForRoom(roomId, id);

            if (itemOfInterest == null)
            {
                return NotFound();
            }

            var itemOfInterestResult = Mapper.Map<ItemOfInterestDto>(itemOfInterest);
            return Ok(itemOfInterestResult);             
        }

        [HttpPost("{roomId}/itemsofinterest")]
        public IActionResult CreateItemOfInterest(int roomId,
            [FromBody] ItemOfInterestForCreationDto itemOfInterest)
        {
            if (itemOfInterest == null)
            {
                return BadRequest();
            }

            if (itemOfInterest.Description == itemOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_roomInfoRepository.RoomExists(roomId))
            {
                return NotFound();
            }

            var finalItemOfInterest = Mapper.Map<Entities.ItemOfInterest>(itemOfInterest);

            _roomInfoRepository.AddItemOfInterestForRoom(roomId, finalItemOfInterest);

            if (!_roomInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdItemOfInterestToReturn = Mapper.Map<Models.ItemOfInterestDto>(finalItemOfInterest);

            return CreatedAtRoute("GetItemOfInterest", new
            { roomId = roomId, id = createdItemOfInterestToReturn.Id }, createdItemOfInterestToReturn);
        }

        [HttpPut("{roomId}/itemsofinterest/{id}")]
        public IActionResult UpdateItemOfInterest(int roomId, int id,
            [FromBody] ItemOfInterestForUpdateDto itemOfInterest)
        {
            if (itemOfInterest == null)
            {
                return BadRequest();
            }

            if (itemOfInterest.Description == itemOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_roomInfoRepository.RoomExists(roomId))
            {
                return NotFound();
            }

            var itemOfInterestEntity = _roomInfoRepository.GetItemOfInterestForRoom(roomId, id);
            if (itemOfInterestEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(itemOfInterest, itemOfInterestEntity);

            if (!_roomInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }


        [HttpPatch("{roomId}/itemsofinterest/{id}")]
        public IActionResult PartiallyUpdateItemOfInterest(int roomId, int id,
            [FromBody] JsonPatchDocument<ItemOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_roomInfoRepository.RoomExists(roomId))
            {
                return NotFound();
            }

            var itemOfInterestEntity = _roomInfoRepository.GetItemOfInterestForRoom(roomId, id);
            if (itemOfInterestEntity == null)
            {
                return NotFound();
            }

            var itemOfInterestToPatch = Mapper.Map<ItemOfInterestForUpdateDto>(itemOfInterestEntity);

            patchDoc.ApplyTo(itemOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (itemOfInterestToPatch.Description == itemOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            TryValidateModel(itemOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(itemOfInterestToPatch, itemOfInterestEntity);

            if (!_roomInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpDelete("{roomId}/itemsofinterest/{id}")]
        public IActionResult DeleteItemOfInterest(int roomId, int id)
        {
            if (!_roomInfoRepository.RoomExists(roomId))
            {
                return NotFound();
            }

            var itemOfInterestEntity = _roomInfoRepository.GetItemOfInterestForRoom(roomId, id);
            if (itemOfInterestEntity == null)
            {
                return NotFound();
            }

            _roomInfoRepository.DeleteItemOfInterest(itemOfInterestEntity);

            if (!_roomInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            _logger.LogInformation("Item of interest deleted.",
                    $"Item of interest {itemOfInterestEntity.Name} with id {itemOfInterestEntity.Id} was deleted.");
            
            return NoContent();
        }
    }
}
