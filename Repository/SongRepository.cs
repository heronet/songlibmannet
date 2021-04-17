using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repository
{
    public class SongRepository : ISongRepository
    {
        private readonly DataContext _context;
        public SongRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Song>> GetSongsAsync()
        {
            return await _context.Songs.Include(s => s.Artist).Include(s => s.Album).ToListAsync();
        }
        public async Task<Song> GetSongByIdAsync(Guid id)
        {
            Song song = await _context.Songs.FindAsync(id);
            return song;
        }
        public async Task<Song> GetSongByTitleAsync(string title)
        {
            Song song = await _context.Songs.FirstOrDefaultAsync(x => x.Title == title);
            return song;
        }
        public async Task<bool> AddSongAsync(Song song)
        {
            _context.Songs.Add(song);
            return (await _context.SaveChangesAsync() > 0); // Save successful if returns > 0
        }

        public async Task<bool> DeleteSongAsync(Guid id)
        {
            Song song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
                return (await _context.SaveChangesAsync() > 0);
            }
            return false;
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        // Artists
        public async Task<Artist> GetArtistAsync(string name)
        {
            Artist artist = await _context.Artists.FirstOrDefaultAsync(a => a.Name == name);
            return artist;
        }
        public async Task<Album> GetAlbumAsync(string title)
        {
            Album album = await _context.Albums.FirstOrDefaultAsync(a => a.Title == title);
            return album;
        }
    }
}