using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DXGame.Models.Entities;
using DXGame.Models.Abstract;
using System.Threading.Tasks;

namespace DXGame.Models
{
    public class GameRepository : IPlayroomsRepository, IPlayersRepository, IEventsRepository
    {
        private GameContext db = new GameContext();

        IEnumerable<Playroom> IPlayroomsRepository.Playrooms { get { return db.Playrooms; } }

        IEnumerable<Player> IPlayersRepository.Players { get { return db.Players; } }

        IEnumerable<DXEvent> IEventsRepository.Events { get { return db.Events; } }

        public async Task<Playroom> IPlayroomsRepository.AddAsync(Playroom playroom)
        {

        }

        public async Task<Player> IPlayersRepository.AddAsync(Player player)
        {

        }

        public async Task<DXEvent> IEventsRepository.AddAsync(DXEvent dxEvent)
        {

        }

        public async Task<Playroom> IPlayroomsRepository.AddPlayerToPlayroomAsync(Player player, string playroomName)
        {

        }

        public async Task<Playroom> IPlayroomsRepository.DeleteAsync(string name)
        {

        }

        public async Task<Player> IPlayersRepository.DeleteAsync(string name)
        {

        }

        public async Task<DXEvent> IEventsRepository.DeleteAsync(string name)
        {

        }

        public async Task<Playroom> IPlayroomsRepository.FindAsync(string name)
        {

        }

        public async Task<Player> IPlayersRepository.FindAsync(string name)
        {

        }

        public async Task<Playroom> IPlayroomsRepository.RemovePlayerFromPlayroomAsync(string playerName, string playroomName)
        {

        }

        public async Task<Playroom> IPlayroomsRepository.UpdateAsync(Playroom playroom)
        {

        }

        public async Task<Player> IPlayersRepository.UpdateAsync(Player player)
        {

        }
    }
}