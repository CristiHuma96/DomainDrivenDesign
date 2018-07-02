using FluentNHibernate.Mapping;

namespace Logic.Bookings
{
    public class RoomMapping : ClassMap<Room>
    {
        public RoomMapping()
        {
            Id(x => x.Id);

            Map(x => x.Capacity);
            Map(x => x.RoomType).CustomType<int>();
        }
    }
}