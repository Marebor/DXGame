using System;

namespace DXGame.Common.Messages.Commands.Playroom
{
    public class AddPlayer : ICommand
    {
        public Guid Playroom { get; }
        public Guid Player { get; }

        public AddPlayer(Guid playroom, Guid player)
        {
            this.Playroom = playroom;
            this.Player = player;
        }
    }
}