using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXGame.Providers
{
    public interface IFilenameProvider
    {
        string GenerateFilename(int id, string extension);
    }
}
