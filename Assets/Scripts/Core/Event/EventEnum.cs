﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public enum EventEnum
    {
        GameStart,
        TurnStart,
        TurnEnd,

        OnGameSizeChange,
        AfterGameSizeChange,
    }
}