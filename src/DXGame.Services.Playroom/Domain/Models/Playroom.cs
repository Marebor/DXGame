using System;
using System.Collections.Generic;
using System.Linq;
using DXGame.Common.Exceptions;
using DXGame.Common.Models;
using DXGame.Messages.Abstract;
using DXGame.Messages.Commands.Playroom;
using DXGame.Messages.Events.Game;
using DXGame.Messages.Events.Player;
using DXGame.Messages.Events.Playroom;

namespace DXGame.Services.Playroom.Domain.Models
{
    public class Playroom : Aggregate
    {
        protected override void RegisterAppliers()
        {
            RegisterApplier<PlayroomCreationRequested>(this.Apply);
            RegisterApplier<PlayroomCreationRequestRejected>(this.Apply);
            RegisterApplier<PlayroomCreated>(this.Apply);
            RegisterApplier<PlayerJoined>(this.Apply);
            RegisterApplier<PlayerLeft>(this.Apply);
            RegisterApplier<GameStartRequested>(this.Apply);
            RegisterApplier<GameStarted>(this.Apply);
            RegisterApplier<GameStartFailed>(this.Apply);
            RegisterApplier<GameFinishReceived>(this.Apply);
            RegisterApplier<PrivacyChanged>(this.Apply);
            RegisterApplier<Messages.Events.Playroom.PasswordChanged>(this.Apply);
            RegisterApplier<OwnerChanged>(this.Apply);
            RegisterApplier<PlayroomDeleted>(this.Apply);
        }
        private ISet<Guid> _players { get; set; } = new HashSet<Guid>();
        private ISet<Game> _completedGames { get; set; } = new HashSet<Game>();
        public string Name { get; protected set; }
        public bool IsPrivate { get; protected set; }
        public PlayroomCreationState CreationState { get; protected set; }
        public Guid Owner { get; protected set; }
        public string Password { get; protected set; }
        public IEnumerable<Guid> Players 
        {
            get { return _players; }
            protected set { _players = new HashSet<Guid>(value); }
        }
        public IEnumerable<Game> CompletedGames 
        {
            get { return _completedGames; }
            protected set { _completedGames = new HashSet<Game>(value); }
        }

        public Game ActiveGame { get; protected set; }

        public Playroom() {}
#region Handling Commands
        public static Playroom Create(CreatePlayroom command) 
        {
            var playroom = new Playroom();
            playroom.ApplyEvent(
                new PlayroomCreationRequested(
                    command.PlayroomId, 
                    command.Name,
                    command.IsPrivate,
                    command.Owner,
                    command.Password,
                    playroom.Version,
                    command.CommandId
                )
            );

            return playroom;
        }

        public void RemovePlayer(RemovePlayer command) 
        {
            if (Owner != default(Guid) && command.Requester != Owner)
                throw new DXGameException("unathorized_request");
            if (!_players.Any(p => p == command.Player))
                throw new DXGameException("playroom_does_not_contain_specified_player");

            ApplyEvent(new PlayerLeft(this.Id, command.Player, Version, command.CommandId));
        }

        public void NewGame(StartGame command) 
        {
            if (CompletedGames.Any(g => g.Id == command.Game) || ActiveGame?.Id == command.Game)
                throw new DXGameException("game_already_started");
            if (ActiveGame != null && ActiveGame.State != GameState.Finished && ActiveGame.State != GameState.None)
                throw new DXGameException("another_game_is_already_in_progress");
            if (_players.Count < 3)
                throw new DXGameException("too_small_amount_of_players");
            if (!_players.Contains(command.Requester))
                throw new DXGameException("unathorized_request");

            ApplyEvent(new GameStartRequested(this.Id, command.Game, command.CommandId));
        }

        public void ChangePrivacy(ChangePrivacy command)
        {
            if (Owner != default(Guid) && command.Requester != Owner)
                throw new DXGameException("unathorized_request");
            if (command.Password != Password)
                throw new DXGameException("invalid_password");

            ApplyEvent(new PrivacyChanged(Id, command.Private, Version, command.CommandId));
        }

        public void ChangePassword(ChangePassword command) 
        {
            if (Owner != default(Guid) && command.Requester != Owner)
                throw new DXGameException("unathorized_request");
            if (command.OldPassword != Password)
                throw new DXGameException("invalid_password");
            if (command.NewPassword == command.OldPassword)
                throw new DXGameException("new_password_equal_to_current");

            ApplyEvent(new Messages.Events.Playroom.PasswordChanged(this.Id, command.NewPassword, Version, command.CommandId));
        }

        public void ChangeOwner(ChangeOwner command)
        {
            if (Owner != default(Guid) && command.Requester != Owner)
                throw new DXGameException("unathorized_request");
            if (command.Password != Password)
                throw new DXGameException("invalid_password");
            if (!_players.Contains(command.NewOwner))
                throw new DXGameException("specified_player_is_not_a_member_of_playroom");

            ApplyEvent(new OwnerChanged(Id, command.NewOwner, Version, command.CommandId));
        }

        public void Delete(DeletePlayroom command)
        {
            if (Owner != default(Guid) && command.Requester != Owner)
                throw new DXGameException("unathorized_request");
            if (command.Password != Password)
                throw new DXGameException("invalid_password");

            ApplyEvent(new PlayroomDeleted(Id, Version, command.CommandId));
        }
#endregion Handling Commands

#region Handling External Events
        public void OnPlayroomCreationRequestAcceptedByPlayerService(PlayroomCreationRequestAccepted e)
        {
            if (CreationState != PlayroomCreationState.CreationPending)
                throw new DXGameException("playroom_is_not_pending_to_be_created");

            ApplyEvent(new PlayroomCreated(e.PlayroomId, this.Name, this.IsPrivate, this.Owner, this.Version, e.RelatedCommand));
        }

        public void OnPlayroomCreationRequestRejectedByPlayerService(PlayroomCreationRequestRejected e)
        {
            if (CreationState != PlayroomCreationState.CreationPending)
                throw new DXGameException("playroom_is_not_pending_to_be_created");

            ApplyEvent(e);
        }

        public void OnPlayerAdditionRequestAcceptedByPlayerService(PlayerAdditionRequestAccepted e)
        {
            ApplyEvent(new PlayerJoined(this.Id, e.PlayerId, Version, e.RelatedCommand));
        }

        public void OnGameStartRequestAccepted(GameStartRequestAccepted e)
        {
            if (ActiveGame.State != GameState.StartRequested)
                throw new DXGameException("no_active_request_to_start_game");
            if (ActiveGame.Id != e.Game)
                throw new DXGameException("another_game_is_requested_to_start");

            ApplyEvent(new GameStarted(Id, e.Game, Version, e.RelatedCommand));
        }

        public void OnGameStartRequestRejected(GameStartRequestRejected e)
        {
            if (ActiveGame.State != GameState.StartRequested)
                throw new DXGameException("no_active_request_to_start_game");
            if (ActiveGame.Id != e.Game)
                throw new DXGameException("another_game_is_requested_to_start");

            ApplyEvent(new GameStartFailed(Id, e.Game, "start_request_rejected", e.RelatedCommand));
        }

        public void OnGameFinished(GameFinished e)
        {
            if (ActiveGame.State != GameState.InProgress)
                throw new DXGameException("no_game_in_progress");
            if (ActiveGame.Id != e.Game)
                throw new DXGameException("another_game_is_currently_active");

            ApplyEvent(new GameFinishReceived(Id, e.Game, Version));
        }
#endregion Handling External Events

#region Event Appliers
        public void Apply(PlayroomCreationRequested e)
        {
            Id = e.Id;
            Name = e.Name;
            IsPrivate = e.IsPrivate;
            Owner = e.Owner;
            Password = e.Password;
            Players = new HashSet<Guid>() { Owner };
            CompletedGames = new HashSet<Game>();
            ActiveGame = null;
            CreationState = PlayroomCreationState.CreationPending;
        }

        public void Apply(PlayroomCreated e) 
        {
            CreationState = PlayroomCreationState.Created;
        }
        
        public void Apply(PlayroomCreationRequestRejected e) 
        {
            CreationState = PlayroomCreationState.CreationRejected;
        }

        public void Apply(PlayerJoined e)
        {
            _players.Add(e.Player);
        }

        public void Apply(PlayerLeft e) 
        {
            _players.Remove(e.Player);
        }

        public void Apply(GameStartRequested e) 
        {
            ActiveGame = new Game(e.Game, GameState.StartRequested);
        }

        public void Apply(GameStarted e)
        {
            ActiveGame.State = GameState.InProgress;
        }

        public void Apply(GameStartFailed e)
        {
            ActiveGame = null;
        }

        public void Apply(GameFinishReceived e)
        {
            _completedGames.Add(ActiveGame);
            ActiveGame = null;
        }

        public void Apply(PrivacyChanged e) 
        {
            IsPrivate = e.IsPrivate;
        }

        public void Apply(Messages.Events.Playroom.PasswordChanged e) 
        {
            Password = e.Password;
        }

        public void Apply(OwnerChanged e)
        {
            Owner = e.Owner;
        }

        public void Apply(PlayroomDeleted e)
        {
            IsDeleted = true;
        }
        #endregion Event Appliers
    }

    public enum PlayroomCreationState
    {
        None,
        CreationPending,
        CreationRejected,
        Created
    }
}
