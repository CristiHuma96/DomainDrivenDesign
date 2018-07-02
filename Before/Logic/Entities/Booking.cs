using System;
using System.Collections.Generic;

namespace Logic.Entities
{
    public class Booking : Entity
    {
        public virtual Customer Customer { get; set; }

        public virtual IList<Room> Rooms { get; set; }

        public virtual DateTime StartDateVacation { get; set; }

        public virtual DateTime EndDateVacation { get; set; }

        public virtual DateTime ExpirationDate { get; set; }

        public DateTime ConfirmedDate { get; set; }

        public bool IsConfirmed { get; set; }

        public decimal Price { get; set; }
        
    }
}