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
    public class AT_N005 : ActionCard
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
            await Effects.UseCard.UseActionCard(game, useWay, this, async (g, r) =>
            {
                await g.GetPlayer(r.PlayersId[0]).ChangeSize(g, -g.twoPointCheck(), this, Owner);
            });
        }

        internal override void OnDraw(Game game, Player player)
        {
            game.EventSystem.Register(EventEnum.AfterPlayrSizeChange, game.GetSeat(player), doeffect_passive);
        }
        internal override void OnDrop(Game game, Player player)
        {
            game.EventSystem.Remove(EventEnum.AfterPlayrSizeChange, doeffect_passive);
        }
        private async Task doeffect_passive(object[] args)
        {
            Game game = args[0] as Game;
            EventData<int> value = args[2] as EventData<int>;
            Player sourcePlayer = args[4] as Player;
            if (value.data < 0 && sourcePlayer != Owner)
            {
                TakeChoiceResponse response = (TakeChoiceResponse)await game.WaitAnswer(new TakeChoiceRequest()
                {
                    PlayerId = Owner.Id,
                    Infos = new List<string>() { "ȡ��", "ʹ�ù���" }
                });
                if (response.Index == 1)
                {
                    await Effects.UseCard.UseActionCard(game, new FreeUse()
                    {
                        PlayerId = Owner.Id,
                        CardId = Id,
                        Source = new List<int>() { Id },
                        PlayersId = new List<int>() { sourcePlayer.Id }
                    }, this, async (g, r) =>
                    {
                        await g.GetPlayer(r.PlayersId[0]).ChangeSize(g, value.data, this, g.GetPlayer(r.PlayerId));
                    });
                }
            }
        }
    }
}