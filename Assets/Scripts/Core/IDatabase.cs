﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public interface IDatabase
    {
        Card Get(int Id);
    }
}
