using System;

namespace Models
{
    public class Song
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int TrackNo { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public Artist Artist { get; set; }
        public Guid ArtistId { get; set; }
        public Album Album { get; set; }
        public Guid AlbumId { get; set; }
    }
}