using System;
using System.Threading.Tasks;
using Data;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class EndUserRepository : IEndUserRepository
    {
        private readonly DataContext _context;
        public EndUserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<EndUser> GetUserByIdAsync(Guid id)
        {
            EndUser user = await _context.EndUsers.FindAsync(id);
            return user;
        }
        public async Task<EndUser> GetUserByEmailAsync(string email)
        {
            EndUser user = await _context.EndUsers.SingleOrDefaultAsync(x => x.Email == email);
            return user;
        }
        public async Task<bool> CreateEndUserAsync(EndUser endUser)
        {
            var user = await _context.EndUsers.AddAsync(endUser);
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<EndUser> GetUserByUsernameAsync(string name)
        {
            EndUser user = await _context.EndUsers.SingleOrDefaultAsync(x => x.Name == name);
            return user;
        }
    }
}