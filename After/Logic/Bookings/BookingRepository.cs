using System;
using System.Collections.Generic;
using System.Linq;
using Logic.SharedKernel;
using Logic.Utils;

namespace Logic.Bookings
{
    public class BookingRepository : Repository<Booking>
    {
        public BookingRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IReadOnlyList<Booking> GetList()
        {
            return _unitOfWork
                .Query<Booking>()
                .ToList();
        }

        public IReadOnlyList<Booking> GetCurrentBookings()
        {
            return _unitOfWork
                .Query<Booking>()
                .Where(x => x.EndDate < DateTime.Now)
                .ToList();
        }
    }
}