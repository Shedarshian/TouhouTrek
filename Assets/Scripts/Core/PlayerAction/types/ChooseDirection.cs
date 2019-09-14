using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    public class ChooseDirectionRequest : Request
    {
    }
    public class ChooseDirectionResponse : Response
    {
        public bool IfForward;
    }
}
