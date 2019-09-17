using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    /// <summary>
    /// 自由用牌
    /// </summary>
    public class UseCardRequest : Request
    {

    }
    /// <summary>
    /// 结束自由用牌
    /// </summary>
    public class EndTurnResponse:Response
    {
        //public override void HandleAction(Game game)
        //{
        //    Player player = game.Players.Find(x => x.Id == PlayerId);
        //    game.EndTurn(player);
        //}
    }
}
