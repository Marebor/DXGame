using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Commands.Player
{
    public class CreatePlayer : ICommand
    {
        public CreatePlayer(Guid commandId, Guid playerId, string name, string password)
        {
            this.CommandId = commandId;
            this.PlayerId = playerId;
            this.Name = name;
            this.Password = password;

        }
        public Guid CommandId { get; protected set; }
        public Guid PlayerId { get; protected set; }
        public string Name { get; protected set; }
        public string Password { get; protected set; }
    }
}