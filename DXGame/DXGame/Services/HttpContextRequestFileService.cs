using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXGame.Services
{
    public class HttpContextRequestFileService : IRequestFileService
    {
        public HttpPostedFile GetFiles()
        {
            var files = HttpContext.Current.Request.Files;
            var filename = files.AllKeys.FirstOrDefault();
            var file = filename != null ? files[filename] : null;

            return file;
        }
    }
}