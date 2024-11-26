using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.Repositories.Interfaces
{
    public interface ICardRepository
    {
        //CREATE
        Task<bool> Create(Card card, Guid? packageId = null);

        Task<bool> CreateCardsForPackage(Package package);
        //READ
        Task<IEnumerable<Card>> GetAll();
        Task<Package> GetAllPackage(Guid packageId);
        //UPDATE
        Task<bool> Update(Card card);

        //DELETE
        Task<bool> Delete(Guid cardId);
    }
}
