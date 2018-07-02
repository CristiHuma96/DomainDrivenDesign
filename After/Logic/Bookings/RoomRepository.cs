using System.Collections.Generic;
using System.Linq;
using Logic.SharedKernel;
using Logic.Utils;

namespace Logic.Bookings
{
    public class RoomRepository : Repository<Room>
    {
        public RoomRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IReadOnlyList<Room> GetList()
        {
            return _unitOfWork
                .Query<Room>()
                .ToList();
        }
    }
}