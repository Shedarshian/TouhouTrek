using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ZMDFQ
{
    using PlayerAction;
    public class RequestTimeoutManager:MonoBehaviour,ITimeManager
    {
        public Game Game { get; set; }
        public bool DoLog = false;
        List<Request> requests = new List<Request>();

        private void Update()
        {
            foreach (var request in requests.ToArray())
            {
                request.RemainTime -= Time.deltaTime;
                if (request.RemainTime < 0)
                {
                    Log.Debug($"{request.PlayerId}超时了{request.GetType().Name}询问");
                    requests.Remove(request);
                    timeoutAnswer(request);
                }
            }
        }

        public void Register(Request request)
        {
            doLog($"注册了{request.PlayerId}的 {request.GetType().Name}事件，超时：{request.TimeOut}s ");
            requests.Add(request);
        }

        public void Cancel(Request request)
        {
            doLog($"取消了{request.PlayerId}的 {request.GetType().Name}事件，剩余：{request.TimeOut}s ");
            requests.Remove(request);
        }

        void timeoutAnswer(Request request)
        {
            Player player = Game.GetPlayer(request.PlayerId);
            switch (request)
            {
                case FreeUseRequest useCardRequest:
                    Game.Answer(new EndFreeUseResponse() { PlayerId = request.PlayerId });
                    break;
                case ChooseSomeCardRequest dropCard:
                    List<ActionCard> cards = new List<ActionCard>();
                    for (int i = 0; i < dropCard.Count; i++)
                    {
                        cards.Add(player.ActionCards[i]);
                    }
                    Game.Answer(new ChooseSomeCardResponse()
                    {
                        PlayerId = request.PlayerId,
                        Cards = cards.Select(x=>x.Id).ToList()
                    });
                    break;
                case ChooseHeroRequest chooseHero:
                    Game.Answer(new ChooseHeroResponse()
                    {
                        PlayerId = request.PlayerId,
                        HeroId = chooseHero.HeroIds[0],
                    });
                    break;
                case ChooseDirectionRequest chooseDirectionRequest:
                    Game.Answer(new ChooseDirectionResponse() { PlayerId = request.PlayerId, IfSet = false, IfForward = true });
                    break;
                default:
                    Log.Warning($"ai未处理的响应类型:{request.GetType()}");
                    break;
            }
        }

        void doLog(string s)
        {
            if (DoLog) Log.Debug(s);
        }
    }
}
