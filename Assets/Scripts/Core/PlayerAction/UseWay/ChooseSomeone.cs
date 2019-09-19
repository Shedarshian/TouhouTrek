using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    /// <summary>
    /// 选择一定数量英雄来用卡
    /// </summary>
    public class ChooseSomeoneRequest : UseWay
    {
        public int Number;
    }
    public class ChooseSomeoneResponse: UseOneCard
    {
        public List<int> Targets;
        public override Task HandleAction(Game game)
        {
            return game.GetPlayer(PlayerId).UseActionCard(game, CardId, this);
        }
    }
}
