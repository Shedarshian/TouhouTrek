using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    public class TakeChoiceRequest:Request
    {
        public List<string> Infos;
    }
    public class TakeChoiceResponse : Response
    {
        public int Index;
    }
}
