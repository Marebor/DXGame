using System;
using System.Collections.Generic;
using System.Linq;
using DXGame.Common.Messages.Events;

namespace DXGame.Common.Models
{
    public abstract class Aggregate
    {
        public Guid Id { get; protected set; }

        public bool IsDeleted { get; protected set; }
    }
}