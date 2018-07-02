using System;
using FluentNHibernate.Mapping;
using Logic.Bookings;

namespace Logic.Mappings
{
    public class BookingMapping : ClassMap<Booking>
    {
        public BookingMapping()
        {
            Id(x => x.Id);

            Map(x => x.Price).CustomType<decimal>().Access.CamelCaseField(Prefix.Underscore);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
            Map(x => x.ExpirationDate);
            Map(x => x.ConfirmationDate).Nullable();

            HasMany(x => x.BookedRooms).Access.CamelCaseField(Prefix.Underscore);

            References(x => x.Customer);
        }
    }
}