using System;

namespace DXGame.ReadModel.Models
{
    public class PlayerProjection : IProjection
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}