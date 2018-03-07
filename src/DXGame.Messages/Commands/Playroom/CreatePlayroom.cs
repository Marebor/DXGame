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
        public Guid CommandId { get; }
        public Guid PlayroomId { get; }
        public string Name { get; }
        public Guid Owner { get; }
        public bool IsPrivate { get; }
        public string Password { get; }
    }
}