using System;
using System.Collections.Generic;
using System.Linq;
using DXGame.Common.Exceptions;

namespace DXGame.Services.Round.Domain.Models
{
    public class Round
    {
        private ISet<Guid> _players = new HashSet<Guid>();
        private ISet<Guid> _cardHandings = new HashSet<Guid>();
        private ISet<Guid> _votes = new HashSet<Guid>();
        public Guid Id { get; protected set; }
        public Guid GameId { get; protected set; }
        public int RoundNo { get; protected set; }
        public bool IsFinished { get; protected set; }
        public IEnumerable<Guid> Players 
        {
            get { return _players; }
            protected set { _players = new HashSet<Guid>(value); }
        }
        public Guid ActivePlayer { get; protected set; }
        public State State { get; protected set; }
        public IEnumerable<Guid> CardHandings 
        {
            get { return _cardHandings; }
            protected set { _cardHandings = new HashSet<Guid>(value); }
        }
        public IEnumerable<Guid> Votes 
        {
            get { return _votes; }
            protected set { _votes = new HashSet<Guid>(value); }
        }

        protected Round() {}

        public static Round Create(Guid id, Guid gameId, int roundNo, IEnumerable<Guid> players, Guid activePlayer) 
        {
            return new Round{
                Id = id,
                GameId = gameId,
                RoundNo = roundNo,
                IsFinished = false,
                Players = players,
                ActivePlayer = activePlayer,
                State = State.CardHanding,
            };
        }

        public void AddPlayer(Guid playerId) 
        {
            if (_players.Any(p => p == playerId))
                throw new DomainException("round_already_contains_specified_player");
            if (State == State.Voting)
                throw new WrongStateException("round_in_voting_state", "Cannot add player in voting state");

            _players.Add(playerId);
        }

        public void RemovePlayer(Guid playerId) 
        {
            if (!_players.Any(p => p == playerId))
                throw new DomainException("round_does_not_contain_specified_player");
            if (State == State.Voting)
                throw new WrongStateException("round_in_voting_state", "Cannot remove player in voting state");

            _players.Remove(playerId);
        }

        public class WrongStateException : DomainException
        {
            internal WrongStateException(string errorCode, string message) : base(errorCode, message)
            {
            }
        }
    }
    public enum State 
    {
        CardHanding,
        Voting
    }
}