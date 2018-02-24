using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DXGame.Providers.Abstract;

namespace DXGame.Providers
{
    public class ServerPathProvider : IRootPathProvider
    {
        public string GetRoot()
        {
            return HttpContext.Current.Server.MapPath(@"~/");
        }
    }
}