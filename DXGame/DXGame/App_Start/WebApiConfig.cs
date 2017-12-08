using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using Unity;
using Unity.Lifetime;
using Unity.Registration;
using DXGame.Models;
using DXGame.Providers;

namespace DXGame
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Konfiguracja i usługi składnika Web API
            var container = new UnityContainer();
            container.RegisterType<ICardsRepository, CardsRepository>();
            container.RegisterType<IRootPathProvider, ServerPathProvider>();
            container.RegisterType<IFilenameProvider, FilenameProvider>();
            container.RegisterType<INewIDProvider, NewIDProvider>();
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
