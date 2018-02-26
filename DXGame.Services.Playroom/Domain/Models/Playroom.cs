using System;
using System.Collections.Generic;
using System.Linq;
using DXGame.Common.Exceptions;
using DXGame.Common.Messages.Events;
using DXGame.Common.Messages.Events.Playroom;
using DXGame.Common.Models;
using DXGame.Services.Playroom.Events;

namespace DXGame.Services.Playroom.Domain.Models
{
    public class Playroom : Aggregate,
        IApplyEvent<Events.PlayroomCreated>,
        IApplyEvent<PlayerJoined>,
        IApplyEvent<PlayerLeft>,
        IApplyEvent<GameAdded>,
        IApplyEvent<PrivacyChanged>,
        IApplyEvent<Events.PasswordChanged>,
        IApplyEvent<PlayroomDeleted>
    {
        private ISet<Guid> _players { get; set; }
        private ISet<Guid> _games { get; set; }
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

        public Playroom() {}

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
                
            ApplyEvent(new PlayerJoined(this.Id, id));
        }

        public void RemovePlayer(Guid id) 
        {
            if (!_players.Any(p => p == id))
                throw new DXGameException("playroom_does_not_contain_specified_player");

            ApplyEvent(new PlayerLeft(this.Id, id));
        }

        public void AddGame(Guid id) 
        {
            if (_games.Any(g => g == id))
                throw new DXGameException("playroom_already_contains_specified_game");

            ApplyEvent(new GameAdded(this.Id, id));
        }

        public void MakePrivate(string password)
        {
            if (IsPrivate) return;
            IsPrivate = true;
            Password = password;
        }

        public void MakePublic() 
        {
            IsPrivate = false;
            Password = null;
        }

        public void ChangePassword(string oldPassword, string newPassword) 
        {
            if (oldPassword != Password)
                throw new DXGameException("invalid_password");

            ApplyEvent(new Events.PasswordChanged(this.Id, newPassword));
        }

        public void ResetPassword(string oldPassword) 
        {
            if (oldPassword != Password)
                throw new DXGameException("invalid_password");
                
            ApplyEvent(new Events.PasswordChanged(this.Id, null));
        }

        public void ApplyEvent(Events.PlayroomCreated e) 
        {
            Id = e.Playroom.Id;
            Name = e.Playroom.Name;
            IsPrivate = e.Playroom.IsPrivate;
            OwnerPlayerId = e.Playroom.OwnerPlayerId;
            Password = e.Playroom.Password;
            Players = e.Playroom.Players;
            Games = e.Playroom.Games;
        }

        public void ApplyEvent(PlayerJoined e)
        {
            _players.Add(e.Player);
        }

        public void ApplyEvent(PlayerLeft e) 
        {
            _players.Remove(e.Player);
        }

        public void ApplyEvent(GameAdded e) 
        {
            _games.Add(e.Game);
        }

        public void ApplyEvent(PrivacyChanged e) 
        {
            IsPrivate = e.IsPrivate;
        }

        public void ApplyEvent(Events.PasswordChanged e) 
        {
            Password = e.Password;
        }

        public void ApplyEvent(PlayroomDeleted e)
        {
            IsDeleted = true;
        }
    }
}
