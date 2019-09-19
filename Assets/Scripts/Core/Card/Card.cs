using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public abstract class Card
    {
        public int Id;
        public string Name;
        public CardTypeEnum CardType;

        //internal abstract PlayerAction.Request GetRequest();

        //internal abstract void DoEffect(Game game, PlayerAction.Response target);

    }

    public enum CardTypeEnum
    {
        Charactor,
        Theme,
        Event,
        Action,
    }
}
