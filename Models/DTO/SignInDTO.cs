using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
    public class SignInDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}