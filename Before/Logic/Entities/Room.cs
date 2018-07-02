using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Logic.Entities
{
    public class Room : Entity
    {
        public virtual bool Availability { get; set; }

        [Range(1, 5)]
        public virtual int Capacity { get; set; }
        public virtual RoomType RoomType { get; set; }
    }
}