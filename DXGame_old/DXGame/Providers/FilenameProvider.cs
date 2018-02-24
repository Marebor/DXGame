using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DXGame.Providers.Abstract;

namespace DXGame.Providers
{
    public class FilenameProvider : IFilenameProvider
    {
        public string GenerateFilename(int id, string extension)
        {
            var id_formatter = $"D{int.MaxValue.ToString().Length}";
            return $"Card_ID-{id.ToString(id_formatter)}{extension}";
        }
    }
}