using System;
using System.Collections.Generic;
using System.Linq;
using DXGame.Common.Exceptions;
using DXGame.Common.Messages.Events;
using DXGame.Common.Messages.Events.Playroom;
using DXGame.Common.Models;

namespace DXGame.Services.Playroom.Domain.Models
{
    public class Playroom : Aggregate,
        IApplyEvent<PlayroomCreated>,
        IApplyEvent<PlayerJoined>,
        IApplyEvent<PlayerLeft>,
        IApplyEvent<GameStartRequested>,
        IApplyEvent<PrivacyChanged>,
        IApplyEvent<PasswordChanged>,
        IApplyEvent<PlayroomDeleted>
    {
        private ISet<Guid> _players { get; set; }
        private ISet<Guid> _games { get; set; }
        public string Name { get; protected set; }
        public bool IsPrivate { get; protected set; }
        public Guid OwnerId { get; protected set; }
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

        public Guid ActiveGame { get; protected set; }
        public GameStatus GameStatus { get; protected set; }

        public Playroom() {}

        public static Playroom Create(Guid id, string name, bool isPrivate, Guid ownerId, string password) 
        {
            var playroom = new Playroom();
            playroom.ApplyEvent(new PlayroomCreated(id, name, isPrivate, ownerId, password));

            return playroom;
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

        public void StartGame(Guid id) 
        {
            if (_games.Any(g => g == id))
                throw new DXGameException("playroom_already_contains_specified_game");
            if (ActiveGame != default(Guid) && GameStatus != GameStatus.Finished)
                throw new DXGameException("another_game_is_already_in_progress");

            ApplyEvent(new GameStartRequested(this.Id, id, Players));
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

            ApplyEvent(new PasswordChanged(this.Id, newPassword));
        }

        public void ResetPassword(string oldPassword) 
        {
            if (oldPassword != Password)
                throw new DXGameException("invalid_password");
                
            ApplyEvent(new PasswordChanged(this.Id, null));
        }

        public void ApplyEvent(PlayroomCreated e) 
        {
            Id = e.Id;
            Name = e.Name;
            IsPrivate = e.IsPrivate;
            OwnerId = e.OwnerId;
            Password = e.Password;
            Players = new HashSet<Guid>() { OwnerId };
            Games = new HashSet<Guid>();
            ActiveGame = default(Guid);
            GameStatus = GameStatus.None;

            AddRecentlyAppliedEvent(e);
        }

        public void ApplyEvent(PlayerJoined e)
        {
            _players.Add(e.Player);

            AddRecentlyAppliedEvent(e);
        }

        public void ApplyEvent(PlayerLeft e) 
        {
            _players.Remove(e.Player);
            
            AddRecentlyAppliedEvent(e);
        }

        public void ApplyEvent(GameStartRequested e) 
        {
            ActiveGame = e.Game;
            GameStatus = GameStatus.StartPending;
            
            AddRecentlyAppliedEvent(e);
        }

        public void ApplyEvent(PrivacyChanged e) 
        {
            IsPrivate = e.IsPrivate;
            
            AddRecentlyAppliedEvent(e);
        }

        public void ApplyEvent(PasswordChanged e) 
        {
            Password = e.Password;
            
            AddRecentlyAppliedEvent(e);
        }

        public void ApplyEvent(PlayroomDeleted e)
        {
            IsDeleted = true;
            
            AddRecentlyAppliedEvent(e);
        }
    }

    public enum GameStatus
    {
        None,
        StartPending,
        InProgress,
        Finished
    }
}
