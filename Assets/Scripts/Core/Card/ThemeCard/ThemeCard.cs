using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public class ThemeCard:Card
    {
        public void Disable(Game game)
        {
            foreach (var effect in Effects)
            {
                effect.Dispose(game);
            }
        }
    }
}
