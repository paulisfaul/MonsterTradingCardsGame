using System;
using System.Collections.Generic;
using System.Text.Json;
using MonsterTradingCardsGame.Application.Handlers.Interfaces;
using MonsterTradingCardsGame.Models.RequestModels;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Services;
using MonsterTradingCardsGame.Helper.HttpServer;
using MonsterTradingCardsGame.Enums;
using BCrypt.Net;


namespace MonsterTradingCardsGame.Application.Handlers
{
    /// <summary>This class implements a handler for user-specific requests.</summary>
    public class AuthenticationHandler : Handler, IHandler
    {
        private readonly AuthenticationService _authenticationService;

        public AuthenticationHandler(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _finalRouteHandlerFunctions.Add("/register", (HandleRegister, null));
            _finalRouteHandlerFunctions.Add("/login", (HandleLogin, null));
        }

        /// <summary>Handles an incoming HTTP request.</summary>
        /// <param name="e">Event arguments.</param>

        private bool HandleRegister(HttpSvrEventArgs e)
        {
            if (e.Method == "POST")
            {
                Console.WriteLine("PlainMessage: " + e.PlainMessage);

                try
                {
                    var userRegisterRequest = JsonSerializer.Deserialize<UserRegisterRequestDto>(e.Payload);
                    if (userRegisterRequest != null)
                    {
                        if (Enum.TryParse<RoleEnum>(userRegisterRequest.Role, true, out var role))
                        {
                            var user = new User(userRegisterRequest.Username, role);
                            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegisterRequest.Password);


                            var result = _authenticationService.Register(user, hashedPassword).Result;
                            e.Reply(result.code, result.message);
                            return result.success;
                        }
                        e.Reply(HttpStatusCode.BAD_REQUEST, "Invalid role.");
                        return false;
                    }
                    e.Reply(HttpStatusCode.BAD_REQUEST, "Invalid data.");
                    return false;
                }
                catch (JsonException)
                {
                    e.Reply(HttpStatusCode.BAD_REQUEST, "Invalid JSON format");
                    return false;
                }
            }
            e.Reply(HttpStatusCode.METHOD_NOT_ALLOWED, "Method not allowed.");
            return false;
        }

        private bool HandleLogin(HttpSvrEventArgs e)
        {
            if (e.Method == "POST")
            {
                Console.WriteLine("PlainMessage: " + e.PlainMessage);

                try
                {
                    var userLoginRequest = JsonSerializer.Deserialize<UserLoginRequestDto>(e.Payload);
                    if (userLoginRequest != null)
                    {
                        var result = _authenticationService.Login(userLoginRequest.Username, userLoginRequest.Password).Result;
                        if (result.success && !String.IsNullOrEmpty(result.token))
                        {
                            var jsonResponse = JsonSerializer.Serialize(new { token = result.token });
                            e.Reply(result.code, jsonResponse);
                            return true;
                        }
                        e.Reply(result.code, "Login failed.");
                        return false;
                    }
                    e.Reply(HttpStatusCode.BAD_REQUEST, "Invalid data.");
                    return false;

                }
                catch (JsonException)
                {
                    e.Reply(HttpStatusCode.BAD_REQUEST, "Invalid JSON format");
                    return false;
                }
            }
            e.Reply(HttpStatusCode.METHOD_NOT_ALLOWED, "Method not allowed.");

            return false;
        }
    }
}