﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Logic.Entities
{
    public class Customer : Entity
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name is too long")]
        public virtual string Name { get; set; }

        [Required]
        [RegularExpression(@"^(.+)@(.S+)$", ErrorMessage = "Email is invalid")]
        public virtual string Email { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public virtual CustomerStatus Status { get; set; }

        public virtual decimal MoneySpent { get; set; }

        public virtual IList<Booking> CurrentBookings { get; set; }

        public virtual IList<Booking> PaidBookings { get; set; }
    }
}
