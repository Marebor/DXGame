using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXGame.Services
{
    public class HttpContextRequestFileService : IRequestFileService
    {
        public HttpFileCollection GetFiles()
        {
            return HttpContext.Current.Request.Files;
        }
    }
}