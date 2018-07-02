using System;

namespace Logic.Bookings
{
    public static class BookingExpiration
    {
        public static DateTime GenerateDate(BookingExpirationType bookingExpirationType = BookingExpirationType.OneDay)
        {
            switch (bookingExpirationType)
            {
                case BookingExpirationType.OneDay:
                    return Short;
                case BookingExpirationType.OneWeek:
                    return Medium;
                case BookingExpirationType.OneMonth:
                    return Long;
                default:
                    throw new ArgumentException("Unknown BookingExpirationType");

            }
        }
        public static DateTime Short = DateTime.Now.AddDays(1);
        public static DateTime Medium = DateTime.Now.AddDays(7);
        public static DateTime Long = DateTime.Now.AddDays(30);
    }

    public enum BookingExpirationType
    {
        OneDay = 1,
        OneWeek = 7,
        OneMonth = 30
    }
}