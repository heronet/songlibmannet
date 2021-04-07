using System;
using System.Threading.Tasks;
using Entities;

namespace Repository
{
    public interface IEndUserRepository
    {
        Task<EndUser> GetUserByIdAsync(Guid id);
        Task<EndUser> GetUserByEmailAsync(string email);
        Task<EndUser> GetUserByUsernameAsync(string name);
        Task<bool> CreateEndUserAsync(EndUser endUser);
    }
}