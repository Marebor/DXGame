using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DXGame.Models.Entities;

namespace DXGame.Models.Abstract
{
    public interface IEventsRepository
    {
        IEnumerable<DXEvent> Events { get; }
        Task<DXEvent> AddAsync(DXEvent dxEvent);
    }
}
