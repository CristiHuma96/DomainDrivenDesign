using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Customers;
using Logic.SharedKernel;

namespace Logic.Bookings
{
    public class Booking : Entity
    {
        public virtual DateTime StartDate { get; }
        public virtual DateTime EndDate { get; }
        public virtual DateTime ExpirationDate { get; }

        public virtual Customer Customer { get; protected set; }
        public virtual DateTime? ConfirmationDate { get; protected set; }

        private decimal _price;
        public virtual Money Price
        {
            get => Money.Of(_price);
            protected set => _price = value;
        }  

        private readonly IList<Room> _bookedRooms;
        public virtual IReadOnlyList<Room> BookedRooms => _bookedRooms.ToList();

        public Booking(Customer customer, DateTime startDate, DateTime endDate, IReadOnlyList<Room> rooms, BookingExpirationType bookingExpirationType, RoomService roomService)
        {
            if (StartDate < EndDate)
                throw new ArgumentException("Invalid booking dates");
            if (!roomService.CheckRoomsAvailability(rooms, startDate, endDate))
                throw new ArgumentException("Booking rooms are unavailable");

            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            StartDate = startDate;
            EndDate = endDate;
            ExpirationDate = BookingExpiration.GenerateDate(bookingExpirationType);
            Price = CalculatePrice();
            _bookedRooms = rooms.ToList();
        }

        public bool AddRoom(Room room)
        {
            if (_bookedRooms.Contains(room))
            {
                throw new ArgumentException("Room has already been booked by this customer");
            }

            if (room.CheckAvailability(StartDate, EndDate))
            {
                throw new ArgumentException("Room has already been booked by another customer");
            }

            _bookedRooms.Add(room);
            return true;
        }

        public Money CalculatePrice()
        {
            decimal basePrice, price = 0;
            switch (Customer.Status.Type)
            {
                case CustomerStatusType.Regular:
                    basePrice = 50;
                    break;

                case CustomerStatusType.Premium:
                    basePrice = 40;
                    break;

                case CustomerStatusType.Vip:
                    basePrice = 30;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            price += _bookedRooms.Sum(bookingRoom => basePrice * (int) bookingRoom.RoomType);

            price *= (int)(EndDate - StartDate).TotalDays;

            return Money.Of(price);
        }

        public void Confirm()
        {
            if (ConfirmationDate == default(DateTime))
                throw new ArgumentException("Booking has already been confirmed");
            if (ExpirationDate < DateTime.Now)
                throw new ArgumentException("Booking has already expired");
            ConfirmationDate = DateTime.Now;
        }

    }
}
