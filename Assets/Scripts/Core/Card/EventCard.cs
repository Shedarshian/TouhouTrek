﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public abstract class EventCard:Card
    {
        public abstract bool ForwardOnly { get; }

        public abstract Task UseForward(Game game, Player user);

        public abstract Task UseBackward(Game game, Player user);
    }
}
