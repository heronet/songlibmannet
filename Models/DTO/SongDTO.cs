using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
    public class SongDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Artist { get; set; }
    }
}