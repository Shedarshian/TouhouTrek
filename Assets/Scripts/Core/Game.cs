﻿using System.Collections;
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

        /// <summary>
        /// 正在结算中的卡
        /// </summary>
        public List<Card> UsingCards = new List<Card>();

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
        public T[] createCards<T>(T origin, int number) where T : Card, new()
        {
            T[] cards = new T[number];
            for (int i = 0; i < number; i++)
            {
                cards[i] = Card.copyCard(origin);
            }
            return cards;
        }
        public async void StartGame(GameOptions options = null)
        {
            if (TimeManager != null)
                TimeManager.Game = this;
            //玩家与玩家初始化
            if (options != null && options.players != null)
                Players = new List<Player>(options.players);
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    Player p;
                    if (i == 1) p = new Player();
                    else { p = new AI(); (p as AI).Init(this); }
                    p.Id = i;
                    p.Hero = new Cards.CR_CP001();
                    Players.Add(p);
                }
            }
            requests = new TaskCompletionSource<Response>[Players.Count];
            Self = Players[1];
            ActivePlayer = Players[0];
            //牌库初始化
            Deck = new List<ActionCard>(options != null && options.actionCards != null ? options.actionCards : createCards(new Cards.AT_N001() { Name = "传教" }, 20));
            ThemeDeck = new List<ThemeCard>(options != null && options.officialCards != null ? options.officialCards : createCards(new Cards.G_001() { Name = "旧作" }, 23));
            EventDeck = new List<EventCard>(options != null && options.eventCards != null ? options.eventCards : createCards(new Cards.EV_E002() { Name = "全国性活动" }, 50));
            //游戏准备阶段
            if (options == null || options.shuffle)
            {
                //TODO:角色牌库洗牌
                Reshuffle(Deck);//行动牌库洗牌
                Reshuffle(ThemeDeck);//官作牌库洗牌
                Reshuffle(EventDeck);//事件牌库洗牌
            }
            //初始化社群规模和影响力
            Size = options != null ? options.initCommunitySize : 0;
            foreach (Player p in Players)
            {
                p.Size = options != null ? options.initInfluence : 0;
            }
            //选择角色
            if (options == null || options.chooseCharacter)
            {
                if (options == null || !options.doubleCharacter)
                {
                    //单角色选择：摸3个角色选1个
                    Task<Response>[] chooseHero = new Task<Response>[Players.Count];
                    for (int i = 0; i < Players.Count; i++)
                    {
                        Player p = Players[i];
                        chooseHero[i] = WaitAnswer(new ChooseHeroRequest() { PlayerId = p.Id, HeroIds = new List<int>() { 1, 2, 3 } });
                    }
                    await Task.WhenAll(chooseHero);
                    Log.Debug($"所有玩家选择英雄完毕！");
                }
                else
                {
                    //TODO:双角色选择：摸6个角色选2个
                }
            }
            //游戏开始时 所有玩家抽两张牌
            foreach (var player in Players)
            {
                player.DrawActionCard(this, 2);
            }
            //开始游戏执行阶段
            EventSystem.Call(EventEnum.GameStart);

            NewTurn(ActivePlayer);
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


            while (true)
            {
                Log.Debug($"玩家{player.Id}出牌中");
                Response response = await WaitAnswer(new UseCardRequest() { PlayerId = player.Id, TimeOut = TurnTime });
                if (response is EndTurnResponse)
                {
                    break;
                }
                else
                {
                    await (response as UseOneCard).HandleAction(this);
                }
            }

            EventSystem.Call(EventEnum.ActionEnd, this);

            var chooseDirectionResponse = (ChooseDirectionResponse)await WaitAnswer(new ChooseDirectionRequest() { PlayerId = player.Id });

            await player.UseEventCard(this, chooseDirectionResponse);

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
            ActiveTheme.Enable(this);
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

        internal int NextInt(int start, int end)
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
    public class GameOptions
    {
        public Player[] players = null;
        public IEnumerable<ActionCard> actionCards = null;
        public IEnumerable<ThemeCard> officialCards = null;
        public IEnumerable<EventCard> eventCards = null;
        public bool shuffle = true;
        public int initCommunitySize = 0;
        public int initInfluence = 0;
        public bool chooseCharacter = true;
        public bool doubleCharacter = false;
    }
}
