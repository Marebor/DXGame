using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DXGame.Models.Entities;

namespace DXGame.Models.Abstract
{
    public interface IPlayersRepository
    {
        IEnumerable<Player> Players { get; }
        Task<Player> AddAsync(Player player);
        Task<Player> DeleteAsync(string name);
        Task<Player> FindAsync(string name);
        Task<Player> UpdateAsync(Player player);
    }
}
