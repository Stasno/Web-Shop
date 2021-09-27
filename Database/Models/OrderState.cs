using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class OrderState
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
    }
}
