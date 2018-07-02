using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Logic.Customers
{
    public class CustomerStatus : ValueObject
    {
        public static readonly CustomerStatus Regular = new CustomerStatus(CustomerStatusType.Regular);

        public CustomerStatusType Type { get; }

        protected CustomerStatus() { }

        private CustomerStatus(CustomerStatusType type): this()
        {
            Type = type;
        }

        public CustomerStatus Promote()
        {
            if (Type < CustomerStatusType.Vip)
            {
                CustomerStatusType newCustomerStatus = Type;
                newCustomerStatus++;
                return new CustomerStatus(newCustomerStatus);
            }

            return this;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
        }
    }


    public enum CustomerStatusType
    {
        Regular = 1,
        Premium = 2,
        Vip = 3
    }
}