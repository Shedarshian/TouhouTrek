using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// ���Σ�ָ��һ����ң��������һ�����ơ�
    /// </summary>
    public class AT_007: ActionCard
    {
        protected override bool canUse(Game game, Request nowRequest, FreeUse useInfo, out NextRequest nextRequest)
        {
            nextRequest = null;
            switch (nowRequest)
            {
                case UseLimitCard useLimitCard:
                    return Effects.UseWayResponse.CheckLimit(game, useLimitCard, useInfo, ref nextRequest, this);
                case FreeUseRequest freeUse:
                    if (useInfo.PlayersId.Count < 1)
                    {
                        nextRequest = new HeroChooseRequest() { };
                        return false;
                    }
                    else
                    {
                        return true;
                    }
            }
            return false;
        }

        public override async Task DoEffect(Game game, FreeUse useWay)
        {
            throw new NotImplementedException();
        }
    }
}