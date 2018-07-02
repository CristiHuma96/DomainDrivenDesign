using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Entities;
using Logic.Utils;

namespace Logic.Repositories
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
                .Where(x => x.EndDateVacation < DateTime.Now)
                .ToList();
        }
    }
}