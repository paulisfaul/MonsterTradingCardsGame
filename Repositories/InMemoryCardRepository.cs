using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Repositories.Interfaces;

namespace MonsterTradingCardsGame.Repositories
{
    public class InMemoryCardRepository : ICardRepository
    {
        private readonly List<Card> _cards;
        public InMemoryCardRepository()
        {
            _cards = new List<Card>();
        }


        public Task AddCardAsync(Card card)
        {
            _cards.Add(card);
            return Task.CompletedTask;
        }

        public Task<Card?> GetAllCardsByPackageIdAsync(Guid packageId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCardAsync(Card card)
        {
            throw new NotImplementedException();

        }

        public Task DeleteCardAsync(Guid cardId)
        {
            var card = _cards.FirstOrDefault(c => c.Id == cardId);
            if (card != null)
            {
                _cards.Remove(card);
            }
            return Task.CompletedTask;
        }

        public Task<Card?> GetCardByIdAsync(Guid cardId)
        {
            var card = _cards.FirstOrDefault(c => c.Id == cardId);
            return Task.FromResult(card);
        }

        public Task<Card?> GetAllCardsByDeckIdAsync(Guid deckId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Create(Card card)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Create(Card card, Guid? packageId = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateCardsForPackage(Package package)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Card>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Package> GetAllPackage(Guid packageId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Card card)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid cardId)
        {
            throw new NotImplementedException();
        }
    }
}
