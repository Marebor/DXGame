using System;
using DXGame.Common.Communication;

namespace DXGame.Messages.Commands.Playroom
{
    public class CreatePlayroom : ICommand
    {
        public Guid PlayroomId { get; }
        public string Name { get; }
        public Guid Owner { get; }
        public bool IsPrivate { get; }
        public string Password { get; }

        public CreatePlayroom(Guid id, string name, Guid owner, bool isPrivate, string password)
        {
            this.PlayroomId = id;
            this.Name = name;
            this.Owner = owner;
            this.IsPrivate = isPrivate;
            this.Password = password;
        }
    }
}