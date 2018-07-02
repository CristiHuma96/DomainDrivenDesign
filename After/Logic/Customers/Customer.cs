using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Logic.Bookings;
using Logic.SharedKernel;

namespace Logic.Customers
{
    public class Customer : Entity
    {
        private string _name;
        public virtual CustomerName Name
        {
            get => (CustomerName)_name;
            set => _name = value;
        }

        private decimal _moneySpent;
        public virtual Money MoneySpent
        {
            get => Money.Of(_moneySpent);
            protected set => _moneySpent = value;
        }

        private readonly string _email;
        public virtual Email Email => (Email)_email;

        public virtual CustomerStatus Status { get; protected set; }

        private readonly IList<Booking> _currentBookings;
        public virtual IReadOnlyList<Booking> CurrentBookings => _currentBookings.ToList();

        private readonly IList<Booking> _paidBookings;
        public virtual IReadOnlyList<Booking> PaidBookings => _paidBookings.ToList();

        protected Customer()
        {
            _currentBookings = new List<Booking>();
            _paidBookings = new List<Booking>();
        }

        public Customer(CustomerName name, Email email) : this()
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _email = email ?? throw new ArgumentNullException(nameof(email));

            MoneySpent = Money.Of(0);
            Status = CustomerStatus.Regular;
        }

        public bool AddBooking(Booking booking)
        {
            if ((CurrentBookings.Contains(booking) || PaidBookings.Contains(booking)))
            {
                _currentBookings.Add(booking);
                return true;
            }

            return false;
        }

        public bool ConfirmBooking(Booking booking)
        {
            booking.Confirm();
            _currentBookings.Remove(booking);
            _paidBookings.Add(booking);

            return true;
        }

        public virtual Result CanPromote()
        {
            if (PaidBookings.Count(x => x.ExpirationDate >= DateTime.UtcNow.AddYears(-1)) < 2)
                return Result.Fail("The customer has to have at least 2 booking in the last year");

            if (PaidBookings.Where(x => x.ConfirmationDate > DateTime.UtcNow.AddYears(-1)).Sum(x => x.Price) < 1000m)
                return Result.Fail("The customer has to have at least 1000 of money spent during the last year");

            return Result.Ok();
        }

        public virtual void Promote()
        {
            if (CanPromote().IsFailure)
                throw new ArgumentException();

            Status = Status.Promote();
        }
    }
}
