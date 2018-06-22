using System;
using DXGame.Messages.Abstract;

namespace DXGame.Messages.Commands.Playroom
{
    public class CreatePlayroom : ICommand
    {
        public CreatePlayroom(Guid commandId, Guid playroomId, string name, Guid owner, bool isPrivate, string password)
        {
            this.CommandId = commandId;
            this.PlayroomId = playroomId;
            this.Name = name;
            this.Owner = owner;
            this.IsPrivate = isPrivate;
            this.Password = password;

        }
        public Guid CommandId { get; protected set; }
        public Guid PlayroomId { get; protected set; }
        public string Name { get; protected set; }
        public Guid Owner { get; protected set; }
        public bool IsPrivate { get; protected set; }
        public string Password { get; protected set; }
    }
}