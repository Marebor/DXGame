using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DXGame.Providers.Abstract;

namespace DXGame.Providers
{
    public class RequestPlayernameProvider : IRequestPlayernameProvider
    {
        private const string HEADER_KEY = "DXGame-Player";

        public string CannotRetrievePlayernameErrorMessage
        {
            get
            {
                return
                    $"Wrong playername: {Environment.NewLine}" +
                    $"- name has to contain even one non-whitespace sign {Environment.NewLine}" +
                    $"- name has to be added to request as a header with key '{HEADER_KEY}'";
            }
        }

        public string GetPlayername()
        {
            if (HttpContext.Current == null) return null;

            string playername = null;
            var headers = HttpContext.Current.Request.Headers;

            if (headers.AllKeys.Contains(HEADER_KEY))
            {
                playername = headers.GetValues(HEADER_KEY).First();
            }

            return playername;
        }
    }
}