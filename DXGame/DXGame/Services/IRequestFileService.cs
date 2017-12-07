using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;

namespace DXGame.Services
{
    public interface IRequestFileService
    {
        HttpFileCollection GetFiles();
    }
}
