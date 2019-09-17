using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public abstract class EventCard:Card
    {
        public abstract bool ForwardOnly { get; }

        public abstract void UseForward(Game game, Player user);

        public abstract void UseBackward(Game game, Player user);
    }
}
