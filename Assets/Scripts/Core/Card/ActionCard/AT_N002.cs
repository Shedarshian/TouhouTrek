using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 创作
    /// 个人影响力+1
    /// </summary>
    public class AT_N002 : ActionCard
    {
        public override bool CanUse(Game game, Request nowRequest, FreeUse useInfo, out UseRequest nextRequest)
        {
            throw new NotImplementedException();
        }

        public override Task DoEffect(Game game, FreeUse useWay)
        {
            throw new NotImplementedException();
        }
    }
}
