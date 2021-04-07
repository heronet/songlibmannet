using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
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
            return await _context.Songs.ToListAsync();
        }
        public async Task<Song> GetSongByIdAsync(Guid id)
        {
            Song song = await _context.Songs.FindAsync(id);
            return song;
        }
        public async Task<bool> AddSongAsync(Song song)
        {
            await _context.Songs.AddAsync(song);
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
    }
}