using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    [Index(nameof(Title))]
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Title { get; set; }
        public int Price { get; set; }

        [ConcurrencyCheck]
        public int InStock { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Description { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();

        [Timestamp]
        public byte[] Timestamp { get; set; }

    }
}
