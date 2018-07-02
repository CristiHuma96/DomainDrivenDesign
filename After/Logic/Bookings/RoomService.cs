using System;
using System.Collections.Generic;

namespace Logic.Bookings
{
    public class RoomService
    {
        public virtual bool CheckRoomsAvailability(IReadOnlyList<Room> rooms, DateTime start, DateTime end)
        {
            //may connect to an external service to check if room available
            return true;
        }
    }
}