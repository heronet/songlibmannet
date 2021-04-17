using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Repository;

namespace Controllers
{
    [Authorize]
    public class SongsController : BaseAPIController
    {
        private readonly ISongRepository _repository;
        public SongsController(ISongRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongDTO>>> GetSongs()
        {
            IEnumerable<Song> songs = await _repository.GetSongsAsync();
            List<SongDTO> songDTOs = new List<SongDTO>();
            SongDTO sdto = new SongDTO();
            foreach (var item in songs)
            {
                sdto = new SongDTO
                {
                    Title = item.Title,
                    Artist = item.Artist.Name,
                    Album = item.Album.Title,
                    Id = item.Id,
                    TrackNo = item.TrackNo
                };
                songDTOs.Add(sdto);
            }

            return Ok(songDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Song>>> GetSong(Guid id)
        {
            Song song = await _repository.GetSongByIdAsync(id);
            if (song != null)
                return Ok(song);
            return BadRequest("Song Not Found");
        }
        [HttpPost]
        public async Task<ActionResult<SongDTO>> AddSong(SongDTO songDTO)
        {

            Song findSong = await _repository.GetSongByTitleAsync(songDTO.Title);
            if (findSong != null) return BadRequest("Song already exists");

            Artist artist = await _repository.GetArtistAsync(songDTO.Artist);
            Album album = await _repository.GetAlbumAsync(songDTO.Album);

            if (artist == null)
            {
                artist = new Artist
                {
                    Name = songDTO.Artist
                };
            }
            if (album == null)
            {
                album = new Album
                {
                    Title = songDTO.Album,
                    Artist = artist
                };
            }

            Song song = new Song
            {
                Title = songDTO.Title,
                Artist = artist,
                Album = album,
                TrackNo = songDTO.TrackNo ?? 0,
                ReleaseDate = songDTO.ReleaseDate
            };
            if (await _repository.AddSongAsync(song))
            {
                return new SongDTO
                {
                    Title = song.Title,
                    Artist = song.Artist.Name,
                    Album = song.Album.Title,
                    TrackNo = song.TrackNo,
                    ReleaseDate = song.ReleaseDate,
                    Id = song.Id
                };
            }
            return BadRequest("Something went wrong");
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteSong(Guid id)
        {
            if (await _repository.DeleteSongAsync(id))
                return Ok();
            return BadRequest("Song Not Found");
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Song>> UpdateSong([FromRoute] Guid id, [FromBody] SongDTO songDTO)
        {
            Song song = await _repository.GetSongByIdAsync(id);
            if (song != null)
            {
                song.Title = songDTO.Title;
                await _repository.SaveChangesAsync();
                return Ok(song);
            }
            return BadRequest("Song Not Found");
        }
    }
}