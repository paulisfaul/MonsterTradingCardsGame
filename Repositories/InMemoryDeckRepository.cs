using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Repositories.Interfaces;

namespace MonsterTradingCardsGame.Repositories
{
    public class InMemoryDeckRepository : IDeckRepository
    {
        public Task<bool> Create(Deck deck)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Deck>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Deck deck)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid deckId)
        {
            throw new NotImplementedException();
        }
    }
}
