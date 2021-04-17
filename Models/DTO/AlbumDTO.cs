using System;
using System.Collections.Generic;

namespace Models.DTO
{
    public class AlbumDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid ArtistId { get; set; }
        public string Artist { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public ICollection<SongDTO> Songs { get; set; }
    }
}