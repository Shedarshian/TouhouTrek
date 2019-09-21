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
    public class AT_N001 : ActionCard
    {
        public override bool CanUse(Game game, Request nowRequest, FreeUse useInfo, out UseRequest nextRequest)
        {
            nextRequest = null;
            switch (nowRequest)
            {
                case UseLimitCard useLimitCard:
                    //TODO 根据Id找到对应类
                    if (useLimitCard.CardType != 0)
                    {
                        return false;
                    }
                    else
                    {
                        if (useInfo.Source.Count != 1)
                        {
                            nextRequest = new CardChooseRequest();
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                case FreeUseRequest freeUse:
                    if (useInfo.Source.Count != 1)
                    {
                        nextRequest = new CardChooseRequest();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
            }
            return false;
        }

        internal override Task DoEffect(Game game, FreeUse useWay)
        {
            return Effects.UseCard.NormalUse(game, useWay, this, effect);          
        }

        private async Task effect(Game game, FreeUse useWay)
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
    }
}
