using System;
using System.Collections.Generic;
using System.Linq;
using DXGame.Common.Exceptions;

namespace DXGame.Services.Playroom.Domain.Models
{
    public class Playroom 
    {
        private ISet<Guid> _players { get; set; }
        private ISet<Guid> _games { get; set; }
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public bool IsPrivate { get; protected set; }
        public Guid OwnerPlayerId { get; protected set; }
        public string Password { get; protected set; }
        public IEnumerable<Guid> Players 
        {
            get { return _players; }
            protected set { _players = new HashSet<Guid>(value); }
        }
        public IEnumerable<Guid> Games 
        {
            get { return _games; }
            protected set { _games = new HashSet<Guid>(value); }
        }

        protected Playroom() {}

        public static Playroom Create(Guid id, string name, bool isPrivate, Guid ownerPlayerId, string password) 
        {
            return new Playroom() 
            {
                Id = id,
                Name = name,
                IsPrivate = isPrivate,
                OwnerPlayerId = ownerPlayerId,
                Password = password,
                Players = new HashSet<Guid>() { ownerPlayerId },
                Games = new HashSet<Guid>(),
            };
        }

        public void AddPlayer(Guid id) 
        {
            if (_players.Any(p => p == id))
                throw new DXGameException("playroom_already_contains_specified_player");

            _players.Add(id);
        }

        public void RemovePlayer(Guid id) 
        {
            if (!_players.Any(p => p == id))
                throw new DXGameException("playroom_does_not_contain_specified_player");

            _players.Remove(id);
        }

        public void AddGame(Guid id) 
        {
            if (_games.Any(g => g == id))
                throw new DXGameException("playroom_already_contains_specified_game");

            _games.Add(id);
        }

        public void MakePrivate() 
        {
            IsPrivate = false;
        }

        public void MakePublic() 
        {
            IsPrivate = true;
        }

        public void SetPassword(string password) 
        {
            Password = password;
        }

        public void ResetPassword() 
        {
            Password = null;
        }
    }
}
