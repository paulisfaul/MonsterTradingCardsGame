using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Repositories.Interfaces;

namespace MonsterTradingCardsGame.Services
{
    public class PackageService
    {
        private readonly IPackageRepository _packageRepository;
        private readonly ICardRepository _cardRepository;

        public PackageService(IPackageRepository packageRepository, ICardRepository cardRepository)
        {
            _packageRepository = packageRepository;
            _cardRepository = cardRepository;
        }

        public async Task<bool> CreatePackage(Package package)
        {
            var success = await _packageRepository.Create(package);
            if (!success)
            {
                return false;
            }

            success = await _cardRepository.CreateCardsForPackage(package);

            return success;
        }
        //public async Task<IEnumerable<User>> GetAllCardsInPackage()
        //{
        //    return await _cardRepository.GetAllCardsByPackageId();
        //}
    }
}