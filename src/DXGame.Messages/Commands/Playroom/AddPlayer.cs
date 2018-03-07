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
        public Guid CommandId { get; }
        public Guid Playroom { get; }
        public Guid Player { get; }
        public string Password { get; }
    }
}