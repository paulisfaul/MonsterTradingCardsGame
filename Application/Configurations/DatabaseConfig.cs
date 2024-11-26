using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Application.Configurations
{
    public static class DatabaseConfig
    {
        public const string ConnectionString = "Host=localhost;Port=5432;Database=DB_MonsterTradingCardsGame;Username=postgres;Password=p_aUl0307";
        public const string SchemaName = "DBSchema_MonsterTradingCardsGame";
    }
}
