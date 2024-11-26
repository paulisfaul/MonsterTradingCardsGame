using System;
using System.Text.Json;
using MonsterTradingCardsGame.Application.Handlers.Interfaces;
using MonsterTradingCardsGame.Helper.HttpServer;
using MonsterTradingCardsGame.Models;
using MonsterTradingCardsGame.Models.RequestModels;
using MonsterTradingCardsGame.Services;

namespace MonsterTradingCardsGame.Application.Handlers
{
    /// <summary>This class implements a handler for user-specific requests.</summary>
    public class PackageHandler : Handler, IHandler
    {
        private readonly PackageService _packageService;
        private readonly AuthenticationService _authenticationService;


        public PackageHandler(PackageService packageService, AuthenticationService authenticationService)
        {
            _packageService = packageService;
            _authenticationService = authenticationService;
            _finalRouteHandlerFunctions.Add("/", (HandlePackages, authenticationService.AuthorizeAdmin));
            //_variableRoute = HandleSingleUserByUsername;
        }

        public bool HandlePackages(HttpSvrEventArgs e)
        {
            if (e.Method == "POST")
            {
                try
                {
                    var packageCreateRequest = JsonSerializer.Deserialize<PackageRequestDto>(e.Payload);
                    if (packageCreateRequest != null)
                    {

                        var package = new Package(packageCreateRequest);

                        var result = _packageService.CreatePackage(package).Result;
                        if (result)
                        {
                            e.Reply(HttpStatusCode.CREATED, "Package erfolgreich erstellt.");
                            return true;
                        }
                        e.Reply(HttpStatusCode.BAD_REQUEST, "Fehler beim Erstellen des Packages.");
                        return false;
                    }

                    e.Reply(HttpStatusCode.BAD_REQUEST, "Ungültige Packagedaten.");
                    return false;
                }
                catch (JsonException)
                {
                    e.Reply(HttpStatusCode.BAD_REQUEST, "Ungültiges JSON-Format.");
                    return false;
                }
            }

            e.Reply(HttpStatusCode.METHOD_NOT_ALLOWED, "Methode nicht erlaubt.");
            return false;
        }

       
    }
}