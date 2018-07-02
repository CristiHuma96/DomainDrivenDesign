using System;
using System.ComponentModel.DataAnnotations;
using Logic.SharedKernel;

namespace Logic.Bookings
{
    public class Room : Entity
    {
        [Range(1, 5)]
        public virtual int Capacity { get; set; }
        public virtual RoomType RoomType { get; set; }
    }

    public enum RoomType
    {
        Single = 1,
        Double = 2,
        Matrimonial = 3,
        Triple = 4,
        Quad = 5
    }
}