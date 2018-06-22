using System;

namespace DXGame.Services.Playroom.Domain.Models
{
    public class Game
    {
        public Guid Id { get; }
        public GameState State { get; set; }

        public Game(Guid id, GameState state)
        {
            this.Id = id;
            this.State = state;
        }
    }

    public enum GameState
    {
        None,
        StartRequested,
        InProgress,
        Finished
    }
}