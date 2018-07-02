using System;
using System.Collections.Generic;
using Logic.Bookings;

namespace Api.Customers
{
    public class CustomerDto
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Email { get; set; }

        public virtual String Status { get; set; }

        public virtual decimal MoneySpent { get; set; }

        public virtual IReadOnlyList<Booking> CurrentBookings { get; set; }

        public virtual IReadOnlyList<Booking> PaidBookings { get; set; }
    }
}