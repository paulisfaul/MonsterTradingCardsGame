using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.Repositories.Interfaces
{
    public interface IDeckRepository
    {
        //CREATE
        Task<bool> Create(Deck deck);
        //READ
        Task<IEnumerable<Deck>> GetAll();
        //UPDATE
        Task<bool> Update(Deck deck);

        //DELETE
        Task<bool> Delete(Guid deckId);

    }
}