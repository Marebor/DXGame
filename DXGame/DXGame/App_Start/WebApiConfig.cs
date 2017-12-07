using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using Unity;
using Unity.Lifetime;
using Unity.Registration;
using DXGame.Models;
using DXGame.Services;

namespace DXGame
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Konfiguracja i usługi składnika Web API
            var container = new UnityContainer();
            var cardsFolder = new FolderCardsRepository("Content/Cards");
            var requestFilesService = new HttpContextRequestFileService();
            container.RegisterInstance<ICardsRepository>(cardsFolder);
            container.RegisterInstance<IRequestFileService>(requestFilesService);
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
