using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Commands.Playroom
{
    public class AddPlayer : ICommand
    {
        public AddPlayer(Guid commandId, Guid playroom, Guid player, string password)
        {
            this.CommandId = commandId;
            this.Playroom = playroom;
            this.Player = player;
            this.Password = password;

        }
        public Guid CommandId { get; protected set; }
        public Guid Playroom { get; protected set; }
        public Guid Player { get; protected set; }
        public string Password { get; protected set; }
    }
}