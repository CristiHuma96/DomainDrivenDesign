using FluentNHibernate.Mapping;
using Logic.Entities;

namespace Logic.Mappings
{
    public class RoomMapping : ClassMap<Room>
    {
        public RoomMapping()
        {
            Id(x => x.Id);

            Map(x => x.Availability);
            Map(x => x.Capacity);
            Map(x => x.RoomType).CustomType<int>();
        }
    }
}