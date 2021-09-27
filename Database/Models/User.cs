using Microsoft.AspNetCore.Identity;

namespace Database.Models
{
    public class User : IdentityUser
    {
        public string Firstname { get; set; }
        public string Secondname { get; set; }
    }
}
