using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DXGame.Models.Entities;

namespace DXGame.Models.Abstract
{
    public interface IPlayroomsRepository
    {
        IEnumerable<Playroom> Playrooms { get; }
        Task<Playroom> AddAsync(string name);
        Task<Playroom> DeleteAsync(string name);
        Task<Playroom> FindAsync(string name);
        Task<Playroom> UpdateAsync(Playroom playroom);
    }
}
