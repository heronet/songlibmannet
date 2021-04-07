using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
    public class SignupDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}