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
        Task<Playroom> AddAsync(Playroom playroom);
        Task<Playroom> DeleteAsync(string name);
        Task<Playroom> FindAsync(string name);
        Task<Playroom> UpdateAsync(Playroom playroom);
        Task<Playroom> AddPlayerToPlayroomAsync(Player player, string playroomName);
        Task<Playroom> RemovePlayerFromPlayroomAsync(string playerName, string playroomName);
    }
}
