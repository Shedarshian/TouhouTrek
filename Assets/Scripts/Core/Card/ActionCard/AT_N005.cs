using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;


namespace ZMDFQ.Cards
{
    /// <summary>
    /// ���ѣ� ָ��һ����ң������һ����������ж�������Ҹ���Ӱ���������ж��������ֵ��
    /// ��������Ҽ�����ĸ���Ӱ����ʱ����ʹ�ã�����ҵĸ���Ӱ����������ͬ����ֵ��
    /// </summary>
    public class AT_005 : ActionCard
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
            int x = game.twoPointCheck();
            await game.GetPlayer(useWay.PlayersId[0]).ChangeSize(game, -x, this);
        }

        internal override void OnDraw(Game game, Player player)
        {
            game.EventSystem.Register(EventEnum.AfterPlayrSizeChange, game.GetSeat(player), effect_passive);
        }
        internal override void OnDrop(Game game, Player player)
        {
            game.EventSystem.Remove(EventEnum.changeEventDirection, effect_passive);
        }

        private Task effect_passive(object[] args)
        {
            //���ﲻ�����첽������ֱ�ӷ����������
            doeffect_passive(args).Start();
            return Task.CompletedTask;
        }

        private async Task doeffect_passive(object[] args)
        {
            Game game = args[0] as Game;
            ChooseDirectionResponse response = args[1] as ChooseDirectionResponse;
            if (!response.IfSet)
            {
                TakeChoiceResponse response1 = (TakeChoiceResponse)await game.WaitAnswer(new TakeChoiceRequest()
                {
                    PlayerId = Owner.Id,
                    Infos = new List<string>() { "ʹ�ù���", "ȡ��" }
                });
                if (response1.Index == 0)
                {
                    await Owner.DropActionCard(game, new List<int>() { this.Id }, true);
                    Player target = args[2].Owner;
                    await target.ChangeSize(game, -x, this);
                }
            }

        }
    }
}