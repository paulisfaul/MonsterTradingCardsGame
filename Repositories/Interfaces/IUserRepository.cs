using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.Repositories.Interfaces
{
    public interface IUserRepository
    {
        //CREATE
        Task<(bool success, int code, string? message)> Create(User user, string hashedPassword);
        //READ
        Task<IEnumerable<User>> GetAll();

        Task<User> GetById(Guid id);
        //Task<User> GetByName(string name);
        Task<(User user, string hashedPassword)> GetByUsername(string username);
        //UPDATE
        Task<bool> Update(User user);

        //DELETE
        Task<bool> Delete(Guid userId);

    }
}
