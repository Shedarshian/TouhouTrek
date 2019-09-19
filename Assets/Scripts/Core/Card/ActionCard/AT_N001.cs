using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 传教：社区规模±2
    /// </summary>
    public class AT_N001 : ActionCard<SimpleRequest, SimpleResponse>
    {
        protected override Task doEffect(Game game, SimpleResponse useWay)
        {
            return Effects.UseCard.NormalUse(game, useWay, this, effect);          
        }

        private async Task effect(Game game, SimpleResponse useWay)
        {
            //询问玩家是加或减
            TakeChoiceResponse response = (TakeChoiceResponse)await game.WaitAnswer(new TakeChoiceRequest()
            {
                PlayerId = useWay.PlayerId,
                Infos = new List<string>()
                {
                    "+2",
                    "-2",
                }
            });
            //处理实际效果
            if (response.Index == 0)
            {
                await game.ChangeSize(2, this);
            }
            else
            {
                await game.ChangeSize(-2, this);
            }
        }

        protected override SimpleRequest useWay()
        {
            return SimpleRequest.Instance;
        }
    }
}
