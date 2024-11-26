using System;
using System.Text.Json;
using MonsterTradingCardsGame.Application.Handlers.Interfaces;
using MonsterTradingCardsGame.Application.Routing;
using MonsterTradingCardsGame.Helper.HttpServer;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Services;

namespace MonsterTradingCardsGame.Application.Handlers
{
    /// <summary>This class implements a handler for user-specific requests.</summary>
    public class UserHandler : Handler, IHandler
    {
        private readonly UserService _userService;

        public UserHandler(UserService userService, AuthenticationService authenticationService)
        {
            _userService = userService;
            _finalRouteHandlerFunctions.Add("/", (HandleUsers, authenticationService.AuthorizeAdmin));
            _variableRoute = HandleSingleUserByUsername;
        }

        /// <summary>Handles an incoming HTTP request to get all users.</summary>
        /// <param name="e">Event arguments.</param>
        public bool HandleUsers(HttpSvrEventArgs e)
        {
            if (e.Method == "GET")
            {
                var users = _userService.GetAllUsers().Result;
                e.Reply(HttpStatusCode.OK, JsonSerializer.Serialize(users));
                return true;
            }
            e.Reply(HttpStatusCode.METHOD_NOT_ALLOWED, "Methode nicht erlaubt.");
            return false;
        }

        /// <summary>Handles an incoming HTTP request to get a user by username.</summary>
        /// <param name="e">Event arguments.</param>
        /// <param name="username">The username extracted from the remaining path.</param>
        public bool HandleSingleUserByUsername(HttpSvrEventArgs e, string currentPath)
        {
            if (e.Method == "GET")
            {
                var segments = e.RemainingPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                if (segments.Length == 0)
                {
                    string username = "";
                    if (currentPath.Length > 1)
                    {
                        username = currentPath.Substring(1);
                    }

                    var user = _userService.GetUserByUsername(username).Result;
                    if (user != null)
                    {
                        e.Reply(HttpStatusCode.OK, JsonSerializer.Serialize(user));
                        return true;
                    }
                    
                    e.Reply(HttpStatusCode.NOT_FOUND, "Benutzer nicht gefunden.");
                    return false;
                }
            }
            e.Reply(400, "Ungültige Anfrage.");
            return false;
        }
    }
}