using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;

namespace Controllers
{
    public class AlbumsController : BaseAPIController
    {
        private readonly DataContext _context;
        public AlbumsController(DataContext context)
        {
            _context = context;
        }
        [HttpGet("{title}")]
        public async Task<ActionResult<AlbumDTO>> GetAlbum(string title)
        {
            Album album = await _context.Albums
                        .Include(a => a.Artist)
                        .Include(s => s.Songs)
                        .FirstOrDefaultAsync(x => x.Title.ToLower() == title.ToLower());
            if (album == null)
                return NotFound("Not found");

            List<SongDTO> songs = new List<SongDTO>();
            foreach (var item in album.Songs)
            {
                var song = new SongDTO
                {
                    Title = item.Title,
                    Artist = item.Artist.Name,
                    Album = item.Album.Title,
                    Id = item.Id,
                    TrackNo = item.TrackNo,
                    ReleaseDate = item.ReleaseDate
                };
                songs.Add(song);
            }
            AlbumDTO albumDTO = new AlbumDTO
            {
                Artist = album.Artist.Name,
                Songs = songs,
                Id = album.Id,
                ArtistId = album.ArtistId,
                Title = album.Title,
                ReleaseDate = album.ReleaseDate
            };
            return Ok(albumDTO);
        }
    }
}