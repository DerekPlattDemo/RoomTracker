using RoomInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomInfo.API
{
    public static class RoomInfoExtensions
    {
        public static void EnsureSeedDataForContext(this RoomInfoContext context)
        {
            if (context.Rooms.Any())
            {
                return;
            }

            // init seed data
            var rooms = new List<Room>()
            {
                new Room()
                {
                     Name = "Conference Room 1",
                     Description = "First floor conference room",
                     ItemsOfInterest = new List<ItemOfInterest>()
                     {
                         new ItemOfInterest() {
                             Name = "Projector 1",
                             Description = "1080p, sn:11111111111111"
                         },
                          new ItemOfInterest() {
                             Name = "Whiteboard",
                             Description = "10 x 20 whiteboard. Covers entire wall opposite projector"
                          },
                     }
                },
                new Room()
                {
                    Name = "Conference Room 2",
                    Description = "2nd floor conference room.",
                    ItemsOfInterest = new List<ItemOfInterest>()
                     {
                         new ItemOfInterest() {
                             Name = "Projector 2",
                             Description = "720p, sn:222222222222222222"
                         },
                          new ItemOfInterest() {
                             Name = "Whiteboard",
                             Description = "5 x 5 whiteboard and 5 colored markers"
                          },
                     }
                },
                new Room()
                {
                    Name = "Studynook 1",
                    Description = "East window with big chair",
                    ItemsOfInterest = new List<ItemOfInterest>()
                     {
                         new ItemOfInterest() {
                             Name = "Leather Chair",
                             Description =  "Comfy leather chair"
                         }
                     }
                },
                new Room()
                {
                    Name = "Studynook 2",
                    Description = "South facing window with desk",
                    ItemsOfInterest = new List<ItemOfInterest>()
                     {
                         new ItemOfInterest() {
                             Name = "Student desk",
                             Description =  "Donated from the university"
                         }
                     }
                }
            };

            context.Rooms.AddRange(rooms);
            context.SaveChanges();
        }
    }
}
