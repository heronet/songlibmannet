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
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
        {
            IEnumerable<Song> songs = await _repository.GetSongsAsync();
            return Ok(songs);
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
        public async Task<ActionResult> AddSong(SongDTO songDTO)
        {
            Song song = new Song
            {
                Title = songDTO.Title,
                Artist = songDTO.Artist
            };
            return Ok(await _repository.AddSongAsync(song));
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
                song.Artist = songDTO.Artist;
                await _repository.SaveChangesAsync();
                return Ok(song);
            }
            return BadRequest("Song Not Found");
        }
    }
}