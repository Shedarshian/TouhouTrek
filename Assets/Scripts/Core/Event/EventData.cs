﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public class EventData<T>
    {
        public T data;
        public EventData()
        {

        }
        public EventData(T t)
        {
            data = t;
        }
    }
}
