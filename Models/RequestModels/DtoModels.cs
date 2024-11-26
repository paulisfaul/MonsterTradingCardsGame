using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Enums;

namespace MonsterTradingCardsGame.Models.RequestModels
{
    public class UserRegisterRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class UserLoginRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class PackageRequestDto : List<CardRequestDto>
    {
    }

    public class CardRequestDto
    { 
        public string Name { get; set; }
        public float Damage { get; set; }

        public string ElementType { get; set; }

        public string CardType { get; set; }

    }
}
