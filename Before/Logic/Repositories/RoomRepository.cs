using System.Collections.Generic;
using System.Linq;
using Logic.Entities;
using Logic.Utils;

namespace Logic.Repositories
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

        public IReadOnlyList<Room> GetFreeRooms()
        {
            return _unitOfWork
                .Query<Room>()
                .Where(x => x.Availability == true)
                .ToList();
        }

        public IReadOnlyList<Room> GetFreeRoomsByType(RoomType roomType)
        {
            return _unitOfWork
                .Query<Room>()
                .Where(x => x.RoomType == roomType && x.Availability == true)
                .ToList();
        }

        public IReadOnlyList<Room> GetFreeRoomsByCapacity(int capacity)
        {
            return _unitOfWork
                .Query<Room>()
                .Where(x => x.Capacity == capacity && x.Availability == true)
                .ToList();
        }
    }
}