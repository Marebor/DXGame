using System;
using System.Collections.Generic;
using System.Linq;
using DXGame.Common.Exceptions;
using DXGame.Messages.Events;
using DXGame.Messages.Events.Playroom;
using DXGame.Common.Models;

namespace DXGame.Services.Playroom.Domain.Models
{
    public class Playroom : Aggregate,
        IApplyEvent<PlayroomCreated>,
        IApplyEvent<PlayerJoined>,
        IApplyEvent<PlayerLeft>,
        IApplyEvent<GameStartRequested>,
        IApplyEvent<GameStarted>,
        IApplyEvent<GameStartFailed>,
        IApplyEvent<PrivacyChanged>,
        IApplyEvent<PasswordChanged>,
        IApplyEvent<OwnerChanged>,
        IApplyEvent<PlayroomDeleted>
    {
        private ISet<Guid> _players { get; set; }
        private ISet<Guid> _games { get; set; }
        public string Name { get; protected set; }
        public bool IsPrivate { get; protected set; }
        public Guid Owner { get; protected set; }
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

        public void AddPlayer(Guid id, string password) 
        {
            if (password != Password)
                throw new DXGameException("invalid_password");
            if (_players.Any(p => p == id))
                throw new DXGameException("playroom_already_contains_specified_player");
                
            ApplyEvent(new PlayerJoined(this.Id, id));
        }

        public void RemovePlayer(Guid id, Guid requester) 
        {
            if (Owner != default(Guid) && requester != Owner)
                throw new DXGameException("unathorized_request");
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
            if (_players.Count < 3)
                throw new DXGameException("too_small_amount_of_players");

            ApplyEvent(new GameStartRequested(this.Id, id, Players));
        }

        public void OnGameStartRequestAccepted(Guid id)
        {
            if (GameStatus != GameStatus.StartRequested)
                throw new DXGameException("no_active_request_to_start_game");
            if (GameStatus == GameStatus.StartRequested && ActiveGame != id)
                throw new DXGameException("another_game_requested_to_start");

            ApplyEvent(new GameStarted(Id, id));
        }

        public void OnGameStartRequestRejected(Guid id)
        {
            if (GameStatus != GameStatus.StartRequested)
                throw new DXGameException("no_active_request_to_start_game");
            if (GameStatus == GameStatus.StartRequested && ActiveGame != id)
                throw new DXGameException("another_game_requested_to_start");

            ApplyEvent(new GameStartFailed(Id, id, "start_request_rejected"));
        }

        public void ChangePrivacy(Guid requester, string password, bool @private)
        {
            if (Owner != default(Guid) && requester != Owner)
                throw new DXGameException("unathorized_request");
            if (password != Password)
                throw new DXGameException("invalid_password");

            ApplyEvent(new PrivacyChanged(Id, @private));
        }

        public void ChangePassword(string oldPassword, string newPassword, Guid requester) 
        {
            if (Owner != default(Guid) && requester != Owner)
                throw new DXGameException("unathorized_request");
            if (oldPassword != Password)
                throw new DXGameException("invalid_password");
            if (newPassword == oldPassword)
                throw new DXGameException("new_password_equal_to_current");

            ApplyEvent(new PasswordChanged(this.Id, newPassword));
        }

        public void ChangeOwner(Guid requester, string password, Guid newOwner)
        {
            if (Owner != default(Guid) && requester != Owner)
                throw new DXGameException("unathorized_request");
            if (password != Password)
                throw new DXGameException("invalid_password");
            if (!_players.Contains(newOwner))
                throw new DXGameException("specified_player_is_not_a_member_of_playroom");

            ApplyEvent(new OwnerChanged(Id, newOwner));
        }

        public void Delete(Guid requester, string password)
        {
            if (Owner != default(Guid) && requester != Owner)
                throw new DXGameException("unathorized_request");
            if (password != Password)
                throw new DXGameException("invalid_password");

            ApplyEvent(new PlayroomDeleted(Id));
        }

        public void ApplyEvent(PlayroomCreated e) 
        {
            Id = e.Id;
            Name = e.Name;
            IsPrivate = e.IsPrivate;
            Owner = e.Owner;
            Password = e.Password;
            Players = new HashSet<Guid>() { Owner };
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
            GameStatus = GameStatus.StartRequested;
            
            AddRecentlyAppliedEvent(e);
        }

        public void ApplyEvent(GameStarted e)
        {
            GameStatus = GameStatus.InProgress;
            _games.Add(e.Game);
            
            AddRecentlyAppliedEvent(e);
        }

        public void ApplyEvent(GameStartFailed e)
        {
            GameStatus = GameStatus.None;
            _games.Remove(e.Game);
            ActiveGame = default(Guid);

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

        public void ApplyEvent(OwnerChanged e)
        {
            Owner = e.Owner;
            
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
        StartRequested,
        InProgress,
        Finished
    }
}
