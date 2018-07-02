using System;
using System.Collections.Generic;
using Logic.Bookings;

namespace Api.Customers
{
    public class CreateBookingDto
    {
        public virtual IReadOnlyList<Room> Rooms { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime EndDate { get; set; }

        public virtual BookingExpirationType BookingExpirationType { get; set; }
    }

}