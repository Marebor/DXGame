using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Http;

namespace DXGame.Extensions
{
    public static class ApiControllerExtensions
    {
        public static T Inject<T>(this ApiController controller)
        {
            return (T)controller.Configuration.DependencyResolver.GetService(typeof(T));
        }
    }
}