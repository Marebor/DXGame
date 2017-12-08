using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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