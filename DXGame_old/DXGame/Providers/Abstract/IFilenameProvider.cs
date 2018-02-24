﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXGame.Providers.Abstract
{
    public interface IFilenameProvider
    {
        string GenerateFilename(int id, string extension);
    }
}
