using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DXGame.Providers.Abstract;

namespace DXGame.Providers
{
    public class RequestPlayroomNameProvider : IRequestPlayroomNameProvider
    {
        private const string HEADER_KEY = "DXGame-Playroom";
        public string GetPlayroomName()
        {
            if (HttpContext.Current == null) return null;

            string name = null;
            var headers = HttpContext.Current.Request.Headers;

            if (headers.AllKeys.Contains(HEADER_KEY))
            {
                name = headers.GetValues(HEADER_KEY).First();
            }

            return name;
        }
    }
}