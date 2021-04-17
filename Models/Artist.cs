using System;
using System.Collections.Generic;

namespace Models
{
    public class Artist
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Album> Albums { get; set; }
        public ICollection<Song> Songs { get; set; }
    }
}