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
    public class Playroom : IEntity
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

        public void MakePrivate(string password)
        {
            IsPrivate = true;
            Password = password;
        }

        public void MakePublic() 
        {
            IsPrivate = false;
            Password = null;
        }

        public void ChangePassword(string newPassword) 
        {
            Password = newPassword;
        }

        public void ResetPassword() 
        {
            Password = null;
        }

        public static IEntity Build(IEnumerable<IEvent> events) 
        {
            var playroom = new Playroom();

            foreach (var e in events) 
            {
                (playroom as dynamic).ApplyEvent(e);
            }

            return playroom;
        }

        private void ApplyEvent(Events.PlayroomCreated e) 
        {
            Id = e.Playroom.Id;
            Name = e.Playroom.Name;
            IsPrivate = e.Playroom.IsPrivate;
            OwnerPlayerId = e.Playroom.OwnerPlayerId;
            Password = e.Playroom.Password;
            Players = e.Playroom.Players;
            Games = e.Playroom.Games;
        }

        private void ApplyEvent(PlayerJoined e)
        {
            AddPlayer(e.Player);
        }

        private void ApplyEvent(PlayerLeft e) 
        {
            RemovePlayer(e.Player);
        }

        private void ApplyEvent(GameAdded e) 
        {
            AddGame(e.Game);
        }

        private void ApplyEvent(PrivacyChanged e) 
        {
            IsPrivate = e.IsPrivate;
        }

        private void ApplyEvent(Events.PasswordChanged e) 
        {
            Password = e.Password;
        }
    }
}
