using System;
using System.Collections.Generic;
using System.Linq;
using DXGame.Core.Domain.Exceptions;

namespace DXGame.Core.Domain.Game
{
    public class Game
    {
        private ISet<Guid> _players { get; set; }
        private ISet<Guid> _rounds { get; set; }
        public Guid Id { get; protected set; }
        public bool IsFinished { get; protected set; }
        public IEnumerable<Guid> Players
        {
            get { return _players; }
            protected set { _players = new HashSet<Guid>(value); }
        }
        public IEnumerable<Guid> Rounds
        {
            get { return _rounds; }
            protected set { _rounds = new HashSet<Guid>(value); }
        }

        protected Game() { }

        public static Game Create(Guid id) 
        {
            return new Game() 
            {
                Id = id,
                IsFinished = false,
                _players = new HashSet<Guid>(),
                _rounds = new HashSet<Guid>(),
            };
        }

        public void AddPlayer(Guid id) 
        {
            if (_players.Any(p => p == id))
                throw new DomainException("game_already_contains_specified_player");

            _players.Add(id);
        }

        public void RemovePlayer(Guid id) 
        {
            if (!_players.Any(p => p == id))
                throw new DomainException("game_does_not_contain_specified_player");

            _players.Remove(id);
        }
        public void AddRound(Guid id) 
        {
            if (_rounds.Any(r => r == id))
                throw new DomainException("game_already_contains_specified_round");

            _rounds.Add(id);
        }

        public void Finish() 
        {
            IsFinished = true;
        }
    }
}