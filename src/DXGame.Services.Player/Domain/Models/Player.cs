using System;
using DXGame.Common.Exceptions;
using DXGame.Common.Models;
using DXGame.Messages.Commands.Player;
using DXGame.Messages.Events.Player;

namespace DXGame.Services.Player.Domain.Models
{
    public class Player : Aggregate
    {
        protected override void RegisterAppliers()
        {
            RegisterApplier<PlayerCreated>(this.Apply);
            RegisterApplier<PasswordChanged>(this.Apply);
        }

        public string Name { get; protected set; }
        public string Password { get; protected set; }

        public Player() {}

#region Handling Commands
        public static Player Create(CreatePlayer command) 
        {
            var player = new Player();
            player.ApplyEvent(
                new PlayerCreated(
                    command.PlayerId, 
                    command.Name,
                    command.Password,
                    player.Version,
                    command.CommandId
                )
            );

            return player;
        }

        public void ChangePassword(ChangePassword command) 
        {
            if (command.Requester != this.Id)
                throw new DXGameException("unathorized_request");
            if (command.OldPassword != Password)
                throw new DXGameException("invalid_password");
            if (command.NewPassword == command.OldPassword)
                throw new DXGameException("new_password_equal_to_current");

            ApplyEvent(new PasswordChanged(this.Id, command.NewPassword, Version, command.CommandId));
        }
#endregion 

#region Event Appliers
        public void Apply(PlayerCreated e) 
        {
            Id = e.Id;
            Name = e.Name;
            Password = e.Password;
        }

        public void Apply(PasswordChanged e) 
        {
            Password = e.Password;
        }
#endregion 
    }
}