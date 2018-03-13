using System;
using System.Collections.Generic;

namespace DXGame.ReadModel.Models
{
    public class PlayroomProjection : IProjection
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public Guid Owner { get; set; }
        public ISet<Guid> Players { get; set; }
        public ISet<Guid> CompletedGames { get; set; }
        public Guid? ActiveGame { get; set; }
    }
}
