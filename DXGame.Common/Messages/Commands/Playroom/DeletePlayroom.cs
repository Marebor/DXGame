using System;

namespace DXGame.Common.Messages.Commands.Playroom
{
    public class DeletePlayroom : ICommand
    {
        public Guid Playroom { get; }
        public string Password { get; }

        public DeletePlayroom(Guid playroom, string password)
        {
            this.Playroom = playroom;
            this.Password = password;
        }
    }
}