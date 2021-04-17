using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
    public class SongDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Artist { get; set; }
        [Required]
        public string Album { get; set; }

        public DateTime? ReleaseDate { get; set; }
        public Guid Id { get; set; }
        [Required]
        public int? TrackNo { get; set; }
        public Guid ArtistId { get; set; }
        public Guid AlbumId { get; set; }
    }
}