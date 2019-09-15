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

        public List<ThemeCard> UsedWeatherDeck = new List<ThemeCard>();

        public List<EventCard> EventDeck = new List<EventCard>();

        public List<EventCard> UsedEventDeck = new List<EventCard>();

        /// <summary>
        /// 当前回合的玩家
        /// </summary>
        public Player ActivePlayer;

        /// <summary>
        /// 自己控制的玩家
        /// </summary>
        public Player Self;

        public System.Action<Game, Request> OnRequest;

        private System.Random ram = new System.Random();

        private TaskCompletionSource<Response> tcs;

        public void StartGame()
        {
            for (int i = 0; i < 10; i++)
            {
                Deck.Add(new ActionCard()
                {
                    Id = i,
                    Name = "创作",
                    RequestWay = SimpleRequest.Instance,
                    Effects = new List<EffectBase>()
                    {
                        new Effect.SimpleScriptingEffect(new Script("api.addPlayerInfluence(api.triggerPlayer,2);"))
                    }
                });
            }
            for (int i = 10; i < 20; i++)
            {
                Deck.Add(new ActionCard()
                {
                    Id = i,
                    Name = "社群+" + (i % 2 + 1),
                    RequestWay = SimpleRequest.Instance,
                    Effects = new List<EffectBase>()
                    {
                        new Effect.ChangeMainSize()
                        {
                            Size =i%2+1
                        }
                    }
                });
            }
            for (int i = 20; i < 40; i++)
            {
                Deck.Add(new ActionCard()
                {
                    Id = i,
                    RequestWay = SimpleRequest.Instance,
                    Name = "社群+2以上时，额外加一",
                    Effects = new List<EffectBase>()
                    {
                        new Effect.MoreSizeChange()
                        {
                            Need=2,
                            Change=1,
                        }
                    }
                });
            }
            Reshuffle(Deck);
            for (int i = 0; i < 8; i++)
            {
                Player p = new Player();
                p.Id = i;
                p.Hero = new Hero() { Name = "Test" + i };
                Players.Add(p);
                p.DrawActionCard(this, 4);
            }
            Self = Players[0];
            ActivePlayer = Players[0];
        }

        public void DoAction(Response action)
        {
            action.HandleAction(this);
        }

        public void Answer(Response target)
        {
            tcs.TrySetResult(target);
        }
        internal Task<Response> WaitAnswer(Request request)
        {
            tcs = new TaskCompletionSource<Response>();
            OnRequest?.Invoke(this, request);
            return tcs.Task;
        }

        internal void NewTurn(Player player)
        {
            player.DrawActionCard(this, 1);
            //WaitAnswer(new TurnStart() { });
        }

        internal async void EndTurn(Player player)
        {
            int max = player.HandMax();
            if (player.Cards.Count > max)
            {
                await WaitAnswer(new DropCardRequest() { Count = player.Cards.Count - max });
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
}
