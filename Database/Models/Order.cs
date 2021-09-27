using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        public int TotalPrice { get; set; }

        [Required]
        [StringLength(11)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(9)]
        public string ZipCode { get; set; }

        [Required]
        [MaxLength(128)]
        public string Address { get; set; }

        [Required]
        [MaxLength(128)]
        public string City { get; set; }

        [Required]
        [MaxLength(128)]
        public string Country { get; set; }

        public DateTime PlacedAt { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public List<OrderOrderState> OrderOrderStates { get; set; } = new List<OrderOrderState>();
    }
}
