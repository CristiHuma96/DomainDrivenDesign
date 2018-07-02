using FluentNHibernate.Mapping;
using Logic.Entities;

namespace Logic.Mappings
{
    public class BookingMapping : ClassMap<Booking>
    {
        public BookingMapping()
        {
            Id(x => x.Id);

            Map(x => x.Customer);
            Map(x => x.Price);
            Map(x => x.StartDateVacation).CustomType<int>();
            Map(x => x.EndDateVacation).CustomType<int>();
        }
    }
}