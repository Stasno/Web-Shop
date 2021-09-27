using System.ComponentModel.DataAnnotations;

namespace WebShop.ViewModels.Order
{
    public class AddOrderRequest
    {
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
    }
}
