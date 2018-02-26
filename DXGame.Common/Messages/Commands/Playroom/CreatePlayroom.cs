using System;

namespace DXGame.Common.Messages.Commands.Playroom
{
    public class CreatePlayroom : ICommand
    {
        public Guid PlayroomId { get; }
        public string Name { get; }
        public Guid OwnerPlayerId { get; }
        public bool IsPrivate { get; }
        public string Password { get; }

        public CreatePlayroom(Guid id, string name, Guid ownerPlayerId, bool isPrivate, string password)
        {
            this.PlayroomId = id;
            this.Name = name;
            this.OwnerPlayerId = ownerPlayerId;
            this.IsPrivate = isPrivate;
            this.Password = password;
        }
    }
}