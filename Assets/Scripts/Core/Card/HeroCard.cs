﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public abstract class HeroCard : Card
    {
        public abstract List<Skill> Skills { get; }
    }
}
