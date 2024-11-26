using System;
using System.Collections.Generic;
using System.Net;
using MonsterTradingCardsGame.Application.Handlers.Interfaces;
using MonsterTradingCardsGame.Application.Routing;
using MonsterTradingCardsGame.Helper.HttpServer;
using HttpStatusCode = MonsterTradingCardsGame.Helper.HttpServer.HttpStatusCode;

namespace MonsterTradingCardsGame.Application.Handlers
{
    /// <summary>This class provides an abstract implementation of the
    /// <see cref="IHandler"/> interface. It also implements static methods
    /// that handles an incoming HTTP request by discovering and calling
    /// available handlers.</summary>
    public  class Handler : IHandler
    {

        protected Dictionary<string, (Func<HttpSvrEventArgs, bool> handle,Func<string, Task<bool>>? authorize)> _finalRouteHandlerFunctions;
        protected Dictionary<string, IHandler> _unfinalRouteHandlers;
        protected Func<HttpSvrEventArgs, string, bool> _variableRoute;

        protected Handler()
        {
            _finalRouteHandlerFunctions = new Dictionary<string, (Func<HttpSvrEventArgs, bool> handle, Func<string, Task<bool>> authorize)>();
            _unfinalRouteHandlers = new Dictionary<string, IHandler>();
        }

        public bool Handle(HttpSvrEventArgs e)
        {
            if (_unfinalRouteHandlers == null && _finalRouteHandlerFunctions == null)
            {
                throw new InvalidOperationException("Route handlers and final routes have not been set.");
            }

            // Extrahiere das erste Segment nach "/api"
            var segments = e.RemainingPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var currentPath = segments.Length > 0 ? "/" + segments[0] : "/";
            e.RemainingPath = segments.Length > 1 ? "/" + string.Join('/', segments, 1, segments.Length - 1) : string.Empty;

            if (_unfinalRouteHandlers != null && _unfinalRouteHandlers.TryGetValue(currentPath, out var handler))
            {
                return handler.Handle(e);
            }

            if (_finalRouteHandlerFunctions != null && _finalRouteHandlerFunctions.TryGetValue(currentPath, out var finalHandler))
            {
                if (finalHandler.authorize != null)
                {
                    var authorizationHeader = e.Headers.FirstOrDefault(header => header.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase));
                    if (authorizationHeader == null)
                    {
                        e.Reply(HttpStatusCode.BAD_REQUEST, "Authorization header is missing.");
                    }
                    string token = authorizationHeader.Value;

                    var authorized = finalHandler.authorize(token).Result;

                    if (!authorized)
                    {
                        e.Reply(HttpStatusCode.UNAUTHORIZED, "JWT invalid.");
                        return false;
                    }
                }
               return finalHandler.handle(e);
                
            }

            if (_variableRoute != null)
            {
                return _variableRoute(e, currentPath);
            }

            e.Reply(HttpStatusCode.BAD_REQUEST, "Bad request.");
            return false;
        }
    }
}