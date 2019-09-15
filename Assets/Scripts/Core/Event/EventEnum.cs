using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public enum EventEnum
    {
        GameStart,
        GameEnd,

        RoundStart,
        RoundEnd,

        TurnStart,
        TurnEnd,

        ActionStart,
        ActionEnd,

        DrawEventCard,

        DrawActionCard,
        DropActionCard,

        OnGameSizeChange,
        AfterGameSizeChange,
    }
}
