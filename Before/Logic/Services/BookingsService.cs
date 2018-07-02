using System;
using System.Linq;
using Logic.Entities;

namespace Logic.Services
{
    public class BookingsService
    {
        private readonly CustomerService _customerService;

        public BookingsService(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public bool CheckBookingDates(Booking booking)
        {
            return (booking.StartDateVacation < DateTime.Now || DateTime.Now <= booking.EndDateVacation);
        }

        public bool CheckRoomsAvailability(Booking booking)
        {
            foreach (Room bookingRoom in booking.Rooms)
            {
                if (bookingRoom.Availability == false)
                    return false;
                return true;
            }
            return true;
        }

        public bool ConfirmBooking(Customer customer, Booking booking)
        {
            if(customer.PaidBookings.Contains(booking))
            {
                return false;
            }

            if (!customer.CurrentBookings.Contains(booking))
            {
                throw new ArgumentException("Customer has no such booking");
            }
           
            customer.CurrentBookings.Remove(booking);
            customer.PaidBookings.Add(booking);
            booking.IsConfirmed = true;
            customer.MoneySpent += booking.Price;

            return true;
        }

        public decimal CalculatePrice(Booking booking)
        {
            decimal basePrice, price = 0;
            switch (booking.Customer.Status)
            {
                case CustomerStatus.Regular:
                    basePrice = 50;
                    break;

                case CustomerStatus.Premium:
                    basePrice = 40;
                    break;

                case CustomerStatus.Vip:
                    basePrice = 30;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (Room bookingRoom in booking.Rooms)
            {
                price += basePrice * (int)bookingRoom.RoomType;
            }

            price *= (int)(booking.EndDateVacation - booking.StartDateVacation).TotalDays;

            return price;
        }
    }
}