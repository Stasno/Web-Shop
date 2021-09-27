using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class OrderOrderState
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int OrderStateId { get; set; }
        public OrderState OrderState { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
