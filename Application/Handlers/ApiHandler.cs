using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Application.Handlers.Interfaces;
using MonsterTradingCardsGame.Helper.HttpServer;

namespace MonsterTradingCardsGame.Application.Handlers
{
    public class ApiHandler: Handler, IHandler
    {
        public ApiHandler(UserHandler userHandler, AuthenticationHandler authenticationHandler, PackageHandler packageHandler)
        {
            _unfinalRouteHandlers.Add("/users", userHandler);
            _unfinalRouteHandlers.Add("/auth", authenticationHandler);
            _unfinalRouteHandlers.Add("/packages", packageHandler);
        }
    }
}
