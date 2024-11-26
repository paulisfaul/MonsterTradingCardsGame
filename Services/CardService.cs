//using MonsterTradingCardsGame.Models;
//using MonsterTradingCardsGame.Repositories.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace MonsterTradingCardsGame.Services
//{
//    public class CardService
//    {
//        private readonly ICardRepository _cardRepository;

//        public CardService(ICardRepository cardRepository)
//        {
//            _cardRepository = cardRepository;
//        }

//        public Task<IEnumerable<Card>> GetCardsByUserIdAsync(Guid userId)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task AddCardAsync(Card card)
//        {
//            // Add any business logic or validation here if needed
//            throw new NotImplementedException();
//        }

//        public Task<Card?> GetCardByIdAsync(Guid cardId) => _cardRepository.GetCardByIdAsync(cardId);

//        public Task DeleteCardAsync(Guid cardId) => _cardRepository.DeleteCardAsync(cardId);
//    }
//}