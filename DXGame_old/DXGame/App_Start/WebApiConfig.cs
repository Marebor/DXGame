using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using Unity;
using Unity.Lifetime;
using Unity.Registration;
using DXGame.Models;
using DXGame.Models.Abstract;
using DXGame.Providers;
using DXGame.Providers.Abstract;

namespace DXGame
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Konfiguracja i usługi składnika Web API
            var container = new UnityContainer();
            container.RegisterType<ICardsRepository, CardsRepository>();
            container.RegisterType<IPlayroomsRepository, GameRepository>();
            container.RegisterType<IPlayersRepository, GameRepository>();
            container.RegisterType<IEventsRepository, GameRepository>();
            container.RegisterType<IRootPathProvider, ServerPathProvider>();
            container.RegisterType<IFilenameProvider, FilenameProvider>();
            container.RegisterType<IBroadcast, SignalRBroadcast>();
            container.RegisterType<IRequestPlayernameProvider, RequestPlayernameProvider>();
            config.DependencyResolver = new UnityResolver(container);

            config.EnableCors();

            // Trasy składnika Web API
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
