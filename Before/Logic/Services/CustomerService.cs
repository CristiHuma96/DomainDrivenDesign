using System;
using System.Linq;
using Logic.Entities;

namespace Logic.Services
{
    public class CustomerService
    {

        public bool AddBooking(Customer customer, Booking booking)
        {
            if ((!customer.CurrentBookings.Contains(booking) || customer.PaidBookings.Contains(booking)))
            {
                customer.CurrentBookings.Add(booking);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PromoteCustomer(Customer customer)
        {
            // at least 2 paid bookings in the last 30 days
            if (customer.PaidBookings.Count(x => x.ExpirationDate >= DateTime.UtcNow.AddDays(-30)) < 2)
                return false;

            // at least 1000 dollars spent during the last year
            if (customer.PaidBookings.Where(x => x.ConfirmedDate > DateTime.UtcNow.AddYears(-1)).Sum(x => x.Price) < 1000m)
                return false;

            customer.Status++;

            return true;
        }
    }
}