using System;

namespace Logic.Entities
{
    public class BookingExpirationType
    {
        public static DateTime Short = DateTime.Now.AddDays(1);
        public static DateTime Medium = DateTime.Now.AddDays(7);
        public static DateTime Long = DateTime.Now.AddDays(30);
    }
}