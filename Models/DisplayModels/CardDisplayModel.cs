using MonsterTradingCardsGame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models.DisplayModels
{
    public class CardDisplayModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Damage { get; set; }
        public ElementTypeEnum ElementType { get; set; }
    }
}
