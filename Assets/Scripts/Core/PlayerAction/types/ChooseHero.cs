using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    public class ChooseHeroRequest:Request
    {
        public List<int> HeroIds;
    }
    public class ChooseHeroResponse : Response
    {
        public int HeroId;
    }
}
