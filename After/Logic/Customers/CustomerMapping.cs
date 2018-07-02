using System;
using FluentNHibernate.Mapping;
using Logic.Customers;

namespace Logic.Mappings
{
    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            Id(x => x.Id);

            Map(x => x.Name).CustomType<string>().Access.CamelCaseField(Prefix.Underscore);
            Map(x => x.Email).CustomType<string>().Access.CamelCaseField(Prefix.Underscore);
            Map(x => x.MoneySpent).CustomType<decimal>().Access.CamelCaseField(Prefix.Underscore);

            Component(x => x.Status, y =>
            {
                y.Map(x => x.Type, "Status").CustomType<int>();
            });

            HasMany(x => x.CurrentBookings).Access.CamelCaseField(Prefix.Underscore);
            HasMany(x => x.PaidBookings).Access.CamelCaseField(Prefix.Underscore);
        }
    }
}
