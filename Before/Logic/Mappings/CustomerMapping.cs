using FluentNHibernate.Mapping;
using Logic.Entities;

namespace Logic.Mappings
{
    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Email);
            Map(x => x.Status).CustomType<int>();
        }
    }
}
