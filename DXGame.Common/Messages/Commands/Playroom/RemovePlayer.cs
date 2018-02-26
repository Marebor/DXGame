using System;

namespace DXGame.Common.Messages.Commands.Playroom
{
    public class RemovePlayer : ICommand
    {
        public Guid Playroom { get; }
        public Guid Player { get; }

        public RemovePlayer(Guid playroom, Guid player)
        {
            this.Playroom = playroom;
            this.Player = player;
        }
    }
}