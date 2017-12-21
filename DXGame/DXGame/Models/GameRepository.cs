using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public IEnumerable<Playroom> Playrooms { get { return db.Playrooms.Include(p => p.Players); } }

        public IEnumerable<Player> Players { get { return db.Players.Include(p => p.Playrooms); } }

        public IEnumerable<DXEvent> Events { get { return db.Events; } }

        async Task<Playroom> IPlayroomsRepository.AddAsync(Playroom playroom)
        {
            if (playroom == null || await (this as IPlayroomsRepository).FindAsync(playroom.Name) != null) return null;
            var entity = db.Playrooms.Add(playroom);
            await db.SaveChangesAsync();

            return entity;
        }

        async Task<Player> IPlayersRepository.AddAsync(Player player)
        {
            if (player == null || await (this as IPlayersRepository).FindAsync(player.Name) != null) return null;
            var entity = db.Players.Add(player);
            await db.SaveChangesAsync();

            return entity;
        }

        async Task<DXEvent> IEventsRepository.AddAsync(DXEvent dxEvent)
        {
            if (dxEvent.ID != 0 && db.Events.FirstOrDefault(e => e.ID == dxEvent.ID) != null) return null;
            var entity =  db.Events.Add(dxEvent);
            await db.SaveChangesAsync();

            return entity;
        }

        async Task<Playroom> IPlayroomsRepository.AddPlayerToPlayroomAsync(Player player, string playroomName)
        {
            var playroom = await (this as IPlayroomsRepository).FindAsync(playroomName);
            if (player == null || playroom == null || playroom.Players.FirstOrDefault(p => p.Name == player.Name) != null) return null;

            playroom.Players.Add(player);
            await db.SaveChangesAsync();

            return playroom;
        }

        async Task<Playroom> IPlayroomsRepository.DeleteAsync(string name)
        {
            var playroom = await (this as IPlayroomsRepository).FindAsync(name);
            if (playroom != null)
            {
                var entity = db.Playrooms.Remove(playroom);
                await db.SaveChangesAsync();
            }

            return playroom;
        }

        async Task<Player> IPlayersRepository.DeleteAsync(string name)
        {
            var player = await (this as IPlayersRepository).FindAsync(name);
            if (player != null)
            {
                var entity = db.Players.Remove(player);
                await db.SaveChangesAsync();
            }

            return player;
        }

        async Task<Playroom> IPlayroomsRepository.FindAsync(string name)
        {
            await Task.Yield();
            return Playrooms.FirstOrDefault(p => p.Name == name);
        }

        async Task<Player> IPlayersRepository.FindAsync(string name)
        {
            await Task.Yield();
            return Players.FirstOrDefault(p => p.Name == name);
        }

        async Task<Playroom> IPlayroomsRepository.RemovePlayerFromPlayroomAsync(string playerName, string playroomName)
        {
            var playroom = await (this as IPlayroomsRepository).FindAsync(playroomName);
            if (playroom == null) return null;
            var player = playroom.Players.FirstOrDefault(p => p.Name == playerName);
            if (player == null) return null;

            playroom.Players.Remove(player);
            await db.SaveChangesAsync();

            return playroom;
        }
    }
}