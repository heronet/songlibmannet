using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Repository
{
    public interface ISongRepository
    {
        Task<IEnumerable<Song>> GetSongsAsync();
        Task<Song> GetSongByIdAsync(Guid id);
        Task<bool> AddSongAsync(Song song);
        Task<bool> DeleteSongAsync(Guid song);
        Task<bool> SaveChangesAsync();
    }
}