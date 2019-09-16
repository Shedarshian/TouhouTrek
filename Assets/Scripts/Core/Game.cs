using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
//using UnityEngine;

namespace ZMDFQ
{
    using PlayerAction;
    public class Game
    {
        public EventSystem EventSystem = new EventSystem();

        public ITimeManager TimeManager;
        /// <summary>
        /// 当前社团规模
        /// </summary>
        public int Size;

        /// <summary>
        /// 当前玩家
        /// </summary>
        public List<Player> Players = new List<Player>();

        public List<ActionCard> Deck = new List<ActionCard>();

        public List<ActionCard> UsedDeck = new List<ActionCard>();

        public List<ThemeCard> ThemeDeck = new List<ThemeCard>();

        public List<ThemeCard> UsedThemeDeck = new List<ThemeCard>();

        public List<EventCard> EventDeck = new List<EventCard>();

        public List<EventCard> UsedEventDeck = new List<EventCard>();

        public ThemeCard ActiveTheme;

        /// <summary>
        /// 当前回合的玩家
        /// </summary>
        public Player ActivePlayer;

        /// <summary>
        /// 自己控制的玩家
        /// </summary>
        public Player Self;

        /// <summary>
        /// 当前处于第几轮
        /// </summary>
        public int Round;

        /// <summary>
        /// 出牌阶段可用时间
        /// </summary>
        public float TurnTime = 30f;

        /// <summary>
        /// 其他询问可用时间
        /// </summary>
        public float RequestTime = 5f;

        public System.Action<Game, Request> OnRequest;

        private System.Random ram = new System.Random();

        /// <summary>
        /// 一名玩家最多处于一个询问状态
        /// </summary>
        private TaskCompletionSource<Response>[] requests;

        public async void StartGame()
        {
            if (TimeManager != null)
                TimeManager.Game = this;

            for (int i = 0; i < 20; i++)
            {
                ActionCard card = new ActionCard()
                {
                    Id = i,
                    Name = "社群+" + (i % 2 + 1),
                    UseWay = SimpleRequest.Instance,
                };
                card.Effects = new List<EffectBase>()
                {
                    new Effect.ChangeMainSize()
                        {
                            Size =i%2+1
                        },
                        new Effect.GoUsedDeck(){ Parent=card},
                };
                Deck.Add(card);
            }
            for (int i = 20; i < 40; i++)
            {
                ActionCard card = new ActionCard()
                {
                    Id = i,
                    UseWay = SimpleRequest.Instance,
                    Name = "社群+2以上时，额外加一",
                };
                card.Effects = new List<EffectBase>()
                {
                    new Effect.MoreSizeChange()
                        {
                            Need=2,
                            Change=1,
                        },
                        new Effect.GoUsedDeck(){ Parent=card},
                };
                Deck.Add(card);
            }
            for (int i = 0; i < 23; i++)
            {
                ThemeDeck.Add(new ThemeCard()
                {
                    Effects = new List<EffectBase>(),
                    Name = "旧作",
                });
            }

            for (int i = 0; i < 50; i++)
            {
                EventCard eventCard = new EventCard()
                {
                    Id = 1000 + i,
                    Name = "事件" + i,
                };
                eventCard.ForwardEffects = new List<EffectBase>()
                {
                    new Effect.ChangeMainSize()
                        {
                            Size =2
                        },
                        new Effect.GoUsedDeck(){ Parent=eventCard},
                };
                eventCard.BackwardEffects = new List<EffectBase>()
                {
                    new Effect.ChangeMainSize()
                        {
                            Size =1
                        },
                        new Effect.GoUsedDeck(){ Parent=eventCard},
                };
                EventDeck.Add(eventCard);
            }

            Reshuffle(Deck);
            for (int i = 0; i < 8; i++)
            {
                Player p;
                if (i == 1) p = new Player();
                else { p = new AI(); (p as AI).Init(this); }
                p.Id = i;
                p.Hero = new Hero() { Name = "Test" + i };
                Players.Add(p);
            }
            requests = new TaskCompletionSource<Response>[Players.Count];
            Self = Players[1];
            ActivePlayer = Players[0];

            Task<Response>[] chooseHero = new Task<Response>[Players.Count];
            for (int i = 0; i < Players.Count; i++)
            {
                Player p = Players[i];
                chooseHero[i] = WaitAnswer(new ChooseHeroRequest() { PlayerId = p.Id, HeroIds = new List<int>() { 1, 2, 3 } });
            }

            await Task.WhenAll(chooseHero);

            Log.Debug($"所有玩家选择英雄完毕！");

            foreach (var player in Players)
            {
                player.DrawActionCard(this, 4);
            }

            EventSystem.Call(EventEnum.GameStart);

            NewTurn(ActivePlayer);
        }

        /// <summary>
        /// 玩家出牌阶段自由出牌用这个接口
        /// </summary>
        /// <param name="action"></param>
        public void DoAction(UseInfo action)
        {
            action.HandleAction(this);
            Answer(action);
        }
   
        /// <summary>
        /// 玩家响应系统询问用这个接口
        /// </summary>
        /// <param name="response"></param>
        public void Answer(Response response)
        {
            int index = Players.FindIndex(x => x.Id == response.PlayerId);
            var tcs = requests[index];
            requests[index] = null;//可能后续会重新对requests[index]询问，所以这个要写在TrySetResult之前
            tcs.TrySetResult(response);
        }
        /// <summary>
        /// 向玩家请求一个动作的回应
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal Task<Response> WaitAnswer(Request request)
        {
            var tcs = new TaskCompletionSource<Response>();
            int index = Players.FindIndex(x => x.Id == request.PlayerId);
            requests[index] = tcs;
            OnRequest?.Invoke(this, request);
            if (TimeManager != null)
            {
                TimeManager.Register(request);
                tcs.Task.ContinueWith(x => { TimeManager.Cancel(request); });
            }
            return tcs.Task;
        }

        internal async void NewTurn(Player player)
        {
            if (Players.IndexOf(player) == 0)
            {
                //新的一轮
                Round++;
                NextThemeCard();
                EventSystem.Call(EventEnum.RoundStart, this);
            }
            EventSystem.Call(EventEnum.TurnStart, this);
            player.DrawEventCard(this);
            player.DrawActionCard(this, 1);
            EventSystem.Call(EventEnum.ActionStart, this);

            UseCardRequest useCardRequest = new UseCardRequest() { PlayerId = player.Id, TimeOut = TurnTime };
            Response response;
            do
            {
                Log.Debug($"玩家{player.Id}出牌中");
                response = await WaitAnswer(useCardRequest);
                Log.Debug($"回合剩余时间{ useCardRequest.TimeOut.ToString()}");
            }
            while (!(response is EndTurnResponse));

            EventSystem.Call(EventEnum.ActionEnd, this);

            var chooseDirectionResponse = (ChooseDirectionResponse)await WaitAnswer(new ChooseDirectionRequest() { PlayerId = player.Id });

            player.UseEventCard(this, chooseDirectionResponse);

            int max = player.HandMax();
            if (player.ActionCards.Count > max)
            {
                ChooseSomeCardResponse chooseSomeCardResponse = (ChooseSomeCardResponse)await WaitAnswer(new ChooseSomeCardRequest() { PlayerId = player.Id, Count = player.ActionCards.Count - max });
                player.DropActionCard(this, chooseSomeCardResponse.Cards);
            }

            EventSystem.Call(EventEnum.TurnEnd, this);

            if (Players.IndexOf(player) == Players.Count - 1)
            {
                //一轮结束了
                EventSystem.Call(EventEnum.RoundEnd, this);
                if (Round + Players.Count == 12)//游戏结束规则
                {
                    EventSystem.Call(EventEnum.GameEnd, this);
                    return;
                }
            }

            int index = Players.IndexOf(player);
            if (index == Players.Count - 1)
            {
                ActivePlayer = Players[0];
            }
            else
            {
                ActivePlayer = Players[index + 1];
            }
            NewTurn(ActivePlayer);
        }

        internal void NextThemeCard()
        {
            if (ActiveTheme != null)
            {
                ActiveTheme.Disable(this);
                UsedThemeDeck.Add(ActiveTheme);
            }
            ActiveTheme = ThemeDeck[0];
            ThemeDeck.RemoveAt(0);
            ActiveTheme.Enable(this, null);
        }

        internal void Reshuffle<T>(List<T> listtemp)
        {
            int currentIndex;
            T tempValue;
            for (int i = 0; i < listtemp.Count; i++)
            {
                currentIndex = ram.Next(0, listtemp.Count - i);
                tempValue = listtemp[currentIndex];
                listtemp[currentIndex] = listtemp[listtemp.Count - 1 - i];
                listtemp[listtemp.Count - 1 - i] = tempValue;
            }
        }

        internal int NextInt(int start,int end)
        {
            return ram.Next(start, end);
        }

        internal void ChangeSize(int size)
        {
            var data = new EventData<int> { data = size };
            EventSystem.Call(EventEnum.OnGameSizeChange, data);
            Size += data.data;
            Log.Debug($"Game size change to {Size}");
        }
    }
}
