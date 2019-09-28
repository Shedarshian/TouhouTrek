﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
//using UnityEngine;

namespace ZMDFQ
{
    using PlayerAction;
    public class Game
    {
        public SeatByEventSystem EventSystem;

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

        /// <summary>
        /// 结算时出过什么卡
        /// </summary>
        public List<UsingInfo> UsingInfos = new List<UsingInfo>();

        List<HeroCard> characterDeck = new List<HeroCard>();

        public List<ActionCard> Deck = new List<ActionCard>();

        public List<ActionCard> UsedActionDeck = new List<ActionCard>();

        public List<ThemeCard> ThemeDeck = new List<ThemeCard>();

        public List<ThemeCard> UsedThemeDeck = new List<ThemeCard>();

        public List<EventCard> EventDeck = new List<EventCard>();

        public List<EventCard> UsedEventDeck = new List<EventCard>();

        public List<EventCard> ChainEventDeck = new List<EventCard>();

        public ThemeCard ActiveTheme;

        /// <summary>
        /// 当前回合的玩家
        /// </summary>
        public Player ActivePlayer;

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

        public Action<Game, Request> OnRequest;

        public Action<Game, Response> OnResponse;

        private System.Random ram = new System.Random();

        private List<Card> allCards = new List<Card>();

        /// <summary>
        /// 一名玩家最多处于一个询问状态
        /// </summary>
        private TaskCompletionSource<Response>[] requests;
        GameOptions options { get; set; } = null;
        int endingOfficialCardCount { get; set; } = 0;
        /// <summary>
        /// 游戏初始化，卡组玩家生成
        /// </summary>
        public void Init(GameOptions options = null)
        {
            EventSystem = new SeatByEventSystem();
            this.options = options;
            if (TimeManager != null)
                TimeManager.Game = this;
            //初始化牌库
            if (options != null && options.characterCards != null)
            {
                characterDeck.AddRange(options.characterCards);
            }
            else
            {
                characterDeck.AddRange(createCards(new Cards.CR_CP001()
                {
                    Name = "传教爱好者",
                }, 28));
            }
            if (options != null && options.actionCards != null)
                Deck.AddRange(options.actionCards);
            else
            {
                Deck.AddRange(createCards(new Cards.AT_N001() { Name = "传教" }, 20));
            }
            if (options != null && options.officialCards != null)
                ThemeDeck.AddRange(options.officialCards);
            else
            {
                ThemeDeck.AddRange(createCards(new Cards.G_001() { Name = "旧作" }, 20));
            }
            if (options != null && options.eventCards != null)
                EventDeck.AddRange(options.eventCards);
            else
            {
                for (int i = 0; i < 50; i++)
                {
                    EventDeck.AddRange(createCards(new Cards.EV_E002() { Name = "全国性活动" }, 20));
                }
            }
            if (options == null || options.shuffle)
            {
                Reshuffle(characterDeck);
                Reshuffle(Deck);
                Reshuffle(ThemeDeck);
                Reshuffle(EventDeck);
            }
            //初始化玩家
            if (options != null && options.players != null)
                Players = new List<Player>(options.players);
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    Player p;
                    if (i == 1) p = new Player(i);
                    else { p = new AI(this, i); }
                    p.Name = "玩家" + i;
                    //p.Hero = new Cards.CR_CP001();
                    Players.Add(p);
                }
            }
            EventSystem.MaxSeat = Players.Count;
            foreach (Player player in Players)
            {
                player.Size = options != null ? options.initInfluence : 0;
            }
            //初始化游戏结束条件
            endingOfficialCardCount = options != null && options.endingOfficialCardCount > 0 ? options.endingOfficialCardCount : 12 - Players.Count;
            requests = new TaskCompletionSource<Response>[Players.Count];
        }

        /// <summary>
        /// 开始游戏流程
        /// </summary>
        public async void StartGame()
        {
            Size = options != null ? options.initCommunitySize : 0;//初始化社群规模
            //Self = Players[1];
            if (options == null || options.firstPlayer == 0)
                ActivePlayer = Players[0];
            else
            {
                int firstPlayerIndex = options.firstPlayer < 0 ? ram.Next(0, Players.Count) : options.firstPlayer;
                var ps = Players.GetRange(0, firstPlayerIndex);
                Players.RemoveRange(0, firstPlayerIndex);
                Players.AddRange(ps);
            }
            if (options == null || options.chooseCharacter)//选择角色
            {
                if (options == null || !options.doubleCharacter)//单角色三选一
                {
                    Task<Response>[] chooseHero = new Task<Response>[Players.Count];
                    for (int i = 0; i < Players.Count; i++)
                    {
                        Player p = Players[i];
                        chooseHero[i] = WaitAnswer(new ChooseHeroRequest() { PlayerId = p.Id, HeroIds = new List<int>(characterDeck.GetRange(i * 3, 3).Select(c => c.Id)) }.SetTimeOut(RequestTime));
                    }

                    await Task.WhenAll(chooseHero);

                    foreach (var response in chooseHero)
                    {
                        var chooseHeroResponse = response.Result as ChooseHeroResponse;
                        Player player = GetPlayer(chooseHeroResponse.PlayerId);
                        player.Hero = characterDeck.Find(c => c.Id == chooseHeroResponse.HeroId);
                        player.Hero.Init(this, player);
                    }
                    Log.Debug($"所有玩家选择英雄完毕！");
                }
                else
                {
                    //TODO:双角色六选二
                }
            }
            //游戏开始时 所有玩家抽两张牌
            foreach (var player in Players)
            {
                await player.DrawActionCard(this, 2);
            }
            //游戏执行阶段
            await EventSystem.Call(EventEnum.GameStart, 0);

            NewRound();
        }
        T[] drawCards<T>(List<T> pile, int number) where T : Card
        {
            T[] drawedCards = new T[pile.Count >= number ? number : pile.Count];
            for (int i = 0; i < drawedCards.Length; i++)
            {
                drawedCards[i] = pile[0];
                pile.RemoveAt(0);
            }
            return drawedCards;
        }
        int lastAllocatedID { get; set; } = 0;
        /// <summary>
        /// 创建卡牌
        /// </summary>
        /// <typeparam name="T">卡牌类型</typeparam>
        /// <param name="origin">数组中的所有卡牌都会以这张牌作为原型</param>
        /// <param name="number">要创建多少张卡牌</param>
        /// <param name="startID">如果不填这个参数那么会自动给卡牌分配ID，如果填了那么被创建的卡牌会从startID开始分配参数，每张卡在前一张的基础上+1</param>
        /// <returns>创建好的卡牌</returns>
        public T[] createCards<T>(T origin, int number, int startID = -1) where T : Card, new()
        {
            T[] cards = new T[number];
            for (int i = 0; i < number; i++)
            {
                cards[i] = Card.copyCard(origin);
                cards[i].Id = startID > -1 ? startID + i : ++lastAllocatedID;
                allCards.Add(cards[i]);
            }
            return cards;
        }
        /// <summary>
        /// 玩家响应系统询问用这个接口
        /// </summary>
        /// <param name="response"></param>
        public void Answer(Response response)
        {
            //Log.Debug(response.GetType().Name);
            int index = Players.FindIndex(x => x.Id == response.PlayerId);
            var tcs = requests[index];
            requests[index] = null;//可能后续会重新对requests[index]询问，所以这个要写在TrySetResult之前
            tcs?.TrySetResult(response);
            OnResponse?.Invoke(this, response);
        }
        /// <summary>
        /// 向玩家请求一个动作的回应
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<Response> WaitAnswer(Request request)
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

        internal async void NewRound()
        {
            Round++;
            //官作发布阶段
            NextThemeCard();
            await EventSystem.Call(EventEnum.RoundStart, 0, this);
            //玩家回合
            for (int i = 0; i < Players.Count; i++)
            {
                await NewTurn(Players[i]);
            }
            //官作弃置阶段
            if (ActiveTheme != null)
            {
                UsedThemeDeck.Add(ActiveTheme);
                if (UsedThemeDeck.Count >= endingOfficialCardCount)//游戏结束规则
                {
                    //游戏结束结算
                    for (int i = 0; i < Players.Count; i++)
                    {
                        Player p = Players[Players.Count - 1 - i];
                        if (p.SaveEvent != null)
                            await p.SaveEvent.Use(this, new ChooseDirectionResponse() { PlayerId = p.Id, CardId = p.SaveEvent.Id, IfForward = true });
                    }
                    //统计玩家胜利情况和得分
                    List<Player> winnerList = new List<Player>();
                    foreach (Player player in Players)
                    {
                        int basePoint = 0;
                        //bool win = true;
                        if (player.Hero.camp == Camp.commuMajor && player.Size >= 0 && Size >= 0)
                            basePoint = Size;
                        else if (player.Hero.camp == Camp.indivMajor && player.Size >= 0 && Size >= 0)
                            basePoint = player.Size;
                        else if (player.Hero.camp == Camp.commuMinor && player.Size >= 0 && Size <= 0)
                            basePoint = Math.Abs(Size);
                        else if (player.Hero.camp == Camp.indivMinor && player.Size >= 0 && Size <= 0)
                            basePoint = player.Size;
                        //else
                        //    win = false;//不结算分的玩家算失败
                        EventData<int> pointData = new EventData<int>() { data = basePoint };
                        //EventData<bool> winData = new EventData<bool>() { data = win };
                        await EventSystem.Call(EventEnum.GetPoint, 0, this, player, pointData/*, winData*/);
                        player.point = pointData.data;
                        if (player.point > 0/*winData.data*/) winnerList.Add(player);
                    }
                    winners = winnerList.ToArray();//这就是获胜者，目前不知道该干嘛。
                    await EventSystem.Call(EventEnum.GameEnd, 0, this);
                    return;
                }
                else
                    ActiveTheme.Disable(this);//在游戏结束的时候不调用该方法，因为此时进入弃牌堆的官作牌被视作有效的。
            }
            //一轮结束了
            await EventSystem.Call(EventEnum.RoundEnd, 0, this);
            NewRound();
        }
        public Player[] winners { get; private set; } = null;
        internal async Task NewTurn(Player player)
        {
            ActivePlayer = player;
            int seat = Players.IndexOf(player);
            await EventSystem.Call(EventEnum.TurnStart, seat, this);
            await player.DrawEventCard(this);
            await player.DrawActionCard(this, 1);
            await EventSystem.Call(EventEnum.ActionStart, seat, this);


            while (true)
            {
                Log.Debug($"玩家{player.Id}出牌中");
                Response response = await WaitAnswer(new FreeUseRequest() { PlayerId = player.Id }.SetTimeOut(TurnTime));
                if (response is EndFreeUseResponse)
                {
                    break;
                }
                else
                {
                    await (response as FreeUse).HandleAction(this);
                }
            }

            await EventSystem.Call(EventEnum.ActionEnd, seat, this);

            var chooseDirectionResponse = (ChooseDirectionResponse)await WaitAnswer(new ChooseDirectionRequest() { PlayerId = player.Id }.SetTimeOut(RequestTime));

            await player.UseEventCard(this, chooseDirectionResponse);

            int max = await player.HandMax(this);
            if (player.ActionCards.Count > max)
            {
                ChooseSomeCardResponse chooseSomeCardResponse = (ChooseSomeCardResponse)await WaitAnswer(new ChooseSomeCardRequest() { PlayerId = player.Id, Count = player.ActionCards.Count - max }.SetTimeOut(RequestTime));
                await player.DropActionCard(this, chooseSomeCardResponse.Cards, true);
            }
            await EventSystem.Call(EventEnum.afterDiscardPhase, seat, this, player);

            await EventSystem.Call(EventEnum.TurnEnd, seat, this);
        }

        internal void NextThemeCard()
        {
            ActiveTheme = ThemeDeck[0];
            ThemeDeck.RemoveAt(0);
            ActiveTheme.Enable(this);
        }

        internal void AddUsingCard(Card card)
        {
            UsingCards.Add(card);
        }
        internal void AddUsingInfo(UsingInfo useInfo)
        {
            UsingInfos.Add(useInfo);
        }

        public int ActivePlayerSeat()
        {
            return Players.IndexOf(ActivePlayer);
        }

        internal Player GetPlayer(int id)
        {
            return Players.Find(x => x.Id == id);
        }

        internal Card GetCard(int id)
        {
            return allCards.Find(x => x.Id == id);
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
        /// <summary>
        /// 改变社群繁荣度
        /// </summary>
        /// <param name="size"></param>
        /// <param name="source">改变的原因</param>
        internal async Task ChangeSize(int size, object source)
        {
            var data = new EventData<int> { data = size };
            await EventSystem.Call(EventEnum.OnGameSizeChange, Players.IndexOf(ActivePlayer), data, source);
            Size += data.data;
            Log.Debug($"Game size change to {Size}");
        }
        public int twoPointCheck()
        {
            switch(ram.Next(1,7))
            {
                case 6:
                case 5:
                    return 2;
                case 4:
                case 3:
                    return 1;
                default:
                    return 0;
            }
        }
    }
    public class GameOptions
    {
        public Player[] players = null;
        public int endingOfficialCardCount = 0;
        public IEnumerable<HeroCard> characterCards = null;
        public IEnumerable<ActionCard> actionCards = null;
        public IEnumerable<ThemeCard> officialCards = null;
        public IEnumerable<EventCard> eventCards = null;
        public int firstPlayer = -1;
        public bool shuffle = true;
        public int initCommunitySize = 0;
        public int initInfluence = 0;
        public bool chooseCharacter = true;
        public bool doubleCharacter = false;
    }
}
