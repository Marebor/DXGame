using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXGame.Services
{
    public interface IFileService
    {
        void SaveFile(Stream data, string filename);
        void DeleteFile(string filename);
    }
}
