using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models.DisplayModels
{
    public class UserDisplayModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Coins { get; set; }

        // Cards in the deck represented as display models
        public DeckDisplayModel Deck { get; set; }

        public StackDisplayModel Stack { get; set; }
    }
}
