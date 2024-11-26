using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.Repositories.Interfaces
{
    public interface IPackageRepository
    {
        //CREATE
        Task<bool> Create(Package package);
        //READ
        Task<IEnumerable<Package>> GetAll();

        Task<Package> GetByUserId(Guid userId);
    }
}
