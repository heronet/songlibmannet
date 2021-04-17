using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;

namespace Controllers
{
    [Authorize]
    public class SongsController : BaseAPIController
    {
        private readonly DataContext _context;
        public SongsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongDTO>>> GetSongs()
        {
            IEnumerable<Song> songs = await _context.Songs
                                            .Include(s => s.Artist)
                                            .Include(s => s.Album)
                                            .ToListAsync();

            if (songs == null) return BadRequest("No Songs Found");

            List<SongDTO> songDTOs = new List<SongDTO>();

            foreach (var song in songs)
                songDTOs.Add(SongToDto(song));
            return Ok(songDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Song>>> GetSong(Guid id)
        {
            Song song = await _context.Songs.FindAsync(id);
            if (song != null)
                return Ok(song);
            return BadRequest("Song Not Found");
        }
        [HttpPost]
        public async Task<ActionResult<SongDTO>> AddSong(SongDTO songDTO)
        {
            Song findSong = await _context.Songs.FirstOrDefaultAsync(x => x.Title == songDTO.Title.ToLower());

            if (findSong != null) return BadRequest("Song already exists");

            Artist artist = await _context.Artists.SingleOrDefaultAsync(x => x.Name == songDTO.Artist.ToLower());
            Album album = await _context.Albums.FirstOrDefaultAsync(x => x.Title == songDTO.Title.ToLower());

            if (artist == null)
            {
                artist = new Artist
                {
                    Name = songDTO.Artist.ToLower().Trim()
                };
            }
            if (album == null)
            {
                album = new Album
                {
                    Title = songDTO.Album.ToLower().Trim(),
                    Artist = artist
                };
            }

            Song song = new Song
            {
                Title = songDTO.Title.ToLower().Trim(),
                Artist = artist,
                Album = album,
                TrackNo = songDTO.TrackNo ?? 0,
                ReleaseDate = songDTO.ReleaseDate
            };
            _context.Add(song);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok(SongToDto(song));
            }
            return BadRequest("Something went wrong");
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSong(Guid id)
        {
            var song = await _context.Songs.FindAsync(id);

            if (song == null) return BadRequest("Song not found");

            _context.Songs.Remove(song);

            if (await _context.SaveChangesAsync() > 0)
                return Ok();

            return BadRequest("Song Not Found");
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Song>> UpdateSong([FromRoute] Guid id, [FromBody] SongDTO songDTO)
        {
            Song song = await _context.Songs.FirstOrDefaultAsync(x => x.Id == id);
            if (song != null)
            {
                song.Title = songDTO.Title;
                await _context.SaveChangesAsync();
                return Ok(SongToDto(song));
            }
            return BadRequest("Song Not Found");
        }


        private SongDTO SongToDto(Song song)
        {
            return new SongDTO
            {
                Title = song.Title,
                Artist = song.Artist.Name,
                Album = song.Album.Title,
                TrackNo = song.TrackNo,
                ReleaseDate = song.ReleaseDate,
                Id = song.Id,
                ArtistId = song.ArtistId,
                AlbumId = song.AlbumId
            };
        }
    }
}