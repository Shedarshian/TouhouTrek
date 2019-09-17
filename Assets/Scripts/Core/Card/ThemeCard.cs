using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public abstract class ThemeCard:Card
    {
        public bool Enabled = false;
        internal abstract void Enable(Game game);
        internal abstract void Disable(Game game);
    }
}
