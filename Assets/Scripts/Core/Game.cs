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

        /// <summary>
        /// 正在结算中的卡
        /// </summary>
        public List<Card> UsingCards = new List<Card>();

        /// <summary>
        /// 结算时出过什么卡
        /// </summary>
        public List<UsingInfo> UsingInfos = new List<UsingInfo>();

        public List<ActionCard> Deck = new List<ActionCard>();

        public List<ActionCard> UsedActionDeck = new List<ActionCard>();

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
        GameOptions options { get; set; } = null;
        /// <summary>
        /// 游戏初始化，卡组玩家生成
        /// </summary>
        public void Init(GameOptions options = null)
        {
            this.options = options;
            if (TimeManager != null)
                TimeManager.Game = this;
            //初始化牌库
            if (options != null && options.actionCards != null)
                Deck = new List<ActionCard>(options.actionCards);
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Deck.Add(new Cards.AT_N001() { Name = "传教" });
                }
            }
            if (options != null && options.officialCards != null)
                ThemeDeck = new List<ThemeCard>(options.officialCards);
            else
            {
                for (int i = 0; i < 23; i++)
                {
                    ThemeDeck.Add(new Cards.G_001() { Name = "旧作" });
                }
            }
            if (options != null && options.eventCards != null)
                EventDeck = new List<EventCard>(options.eventCards);
            else
            {
                for (int i = 0; i < 50; i++)
                {
                    EventDeck.Add(new Cards.EV_E002() { Name = "全国性活动" });
                }
            }
            if (options == null || options.shuffle)
            {
                //TODO:角色牌库洗牌
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
                    //p.Hero = new Cards.CR_CP001();
                    Players.Add(p);
                }
            }
            foreach (Player player in Players)
            {
                player.Size = options != null ? options.initInfluence : 0;
            }
            requests = new TaskCompletionSource<Response>[Players.Count];
        }

        /// <summary>
        /// 开始游戏流程
        /// </summary>
        public async void StartGame()
        {
            Size = options != null ? options.initCommunitySize : 0;//初始化社群规模
            //Self = Players[1];
            //TODO:随机决定玩家行动顺序
            ActivePlayer = Players[0];
            if (options == null || options.chooseCharacter)//选择角色
            {
                if (options == null || !options.doubleCharacter)//单角色三选一
                {
                    Task<Response>[] chooseHero = new Task<Response>[Players.Count];
                    for (int i = 0; i < Players.Count; i++)
                    {
                        Player p = Players[i];
                        chooseHero[i] = WaitAnswer(new ChooseHeroRequest() { PlayerId = p.Id, HeroIds = new List<int>() { 1, 2, 3 } });
                    }

                    await Task.WhenAll(chooseHero);

                    foreach (var response in chooseHero)
                    {
                        var chooseHeroResponse = response.Result as ChooseHeroResponse;
                        GetPlayer(chooseHeroResponse.PlayerId).Hero = new Cards.CR_CP001() { Name = "传教爱好者" };
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
            await EventSystem.Call(EventEnum.GameStart);

            NewTurn(ActivePlayer);
        }
        public T[] createCards<T>(T origin, int number) where T : Card, new()
        {
            T[] cards = new T[number];
            for (int i = 0; i < number; i++)
            {
                cards[i] = Card.copyCard(origin);
            }
            return cards;
        }
        /// <summary>
        /// 玩家响应系统询问用这个接口
        /// </summary>
        /// <param name="response"></param>
        public void Answer(Response response)
        {
            Log.Debug(response.GetType().Name);
            int index = Players.FindIndex(x => x.Id == response.PlayerId);
            var tcs = requests[index];
            requests[index] = null;//可能后续会重新对requests[index]询问，所以这个要写在TrySetResult之前
            tcs?.TrySetResult(response);
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
                await EventSystem.Call(EventEnum.RoundStart, this);
            }
            await EventSystem.Call(EventEnum.TurnStart, this);
            await player.DrawEventCard(this);
            await player.DrawActionCard(this, 1);
            await EventSystem.Call(EventEnum.ActionStart, this);


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

            await EventSystem.Call(EventEnum.ActionEnd, this);

            var chooseDirectionResponse = (ChooseDirectionResponse)await WaitAnswer(new ChooseDirectionRequest() { PlayerId = player.Id });

            await player.UseEventCard(this, chooseDirectionResponse);

            int max = player.HandMax();
            if (player.ActionCards.Count > max)
            {
                ChooseSomeCardResponse chooseSomeCardResponse = (ChooseSomeCardResponse)await WaitAnswer(new ChooseSomeCardRequest() { PlayerId = player.Id, Count = player.ActionCards.Count - max });
                await player.DropActionCard(this, chooseSomeCardResponse.Cards);
            }

            await EventSystem.Call(EventEnum.TurnEnd, this);

            if (Players.IndexOf(player) == Players.Count - 1)
            {
                //一轮结束了
                await EventSystem.Call(EventEnum.RoundEnd, this);
                if (Round + Players.Count == 12)//游戏结束规则
                {
                    await EventSystem.Call(EventEnum.GameEnd, this);
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

        internal void AddUsingCard(Card card)
        {
            UsingCards.Add(card);
        }
        internal void AddUsingInfo(UsingInfo useInfo)
        {
            UsingInfos.Add(useInfo);
        }

        internal Player GetPlayer(int id)
        {
            return Players.Find(x => x.Id == id);
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
            await EventSystem.Call(EventEnum.OnGameSizeChange, data, source);
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
        public int firstPlayer = -1;
        public bool shuffle = true;
        public int initCommunitySize = 0;
        public int initInfluence = 0;
        public bool chooseCharacter = true;
        public bool doubleCharacter = false;
    }
}
