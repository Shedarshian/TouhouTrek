using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 墨菲定理：任意玩家决定事件牌发生方向时使用，你的个人影响力-1，事件牌的发生方向逆转(不影响扣置发生的事件)
    /// </summary>
    public class AT_N020 : ActionCard
    {
        public override string Name => "墨菲定理";
        public override Task DoEffect(Game game, FreeUse useWay)
        {
            return Task.CompletedTask;
        }

        protected override bool canUse(Game game, Request nowRequest, FreeUse useInfo, out NextRequest nextRequest)
        {
            nextRequest = new NextRequest()
            {
                RequestInfo = "无法主动打出"
            };
            return false;
        }

        internal override void OnDraw(Game game, Player player)
        {
            game.EventSystem.Register(EventEnum.changeEventDirection, game.GetSeat(player), effect);
        }

        internal override void OnDrop(Game game, Player player)
        {
            game.EventSystem.Remove(EventEnum.changeEventDirection, effect);
        }

        private Task effect(object[] args)
        {
            //这里不能用异步阻塞，直接返回任务完成
            doeffect(args).Start();
            return Task.CompletedTask;
        }

        private async Task doeffect(object[] args)
        {
            Game game = args[0] as Game;
            ChooseDirectionResponse response = args[1] as ChooseDirectionResponse;
            if (!response.IfSet)
            {
                TakeChoiceResponse response1 = (TakeChoiceResponse)await game.WaitAnswer(new TakeChoiceRequest()
                {
                    PlayerId = Owner.Id,
                    Infos = new List<string>() { "使用墨菲定律", "取消" },
                    AllPlayerRequest = true,
                }.SetTimeOut(game.RequestTime));
                if (response1.Index == 0)
                {
                    //如果使用了墨菲定律，那么取消所有剩余询问
                    game.CancelRequests();
                    await Owner.DropActionCard(game, new List<int>() { this.Id }, true);
                    response.IfForward = !response.IfForward;
                }
            }
        }
    }
}
