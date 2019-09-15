using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    public class ChooseSomeCardRequest :Request
    {
        public int Count;
    }
    public class ChooseSomeCardResponse : Response
    {
        public List<ActionCard> Cards;
    }
}
