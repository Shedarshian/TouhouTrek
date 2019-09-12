using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
//using UnityEngine;

namespace ZMDFQ
{
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

        public Player Self;

        private System.Random ram = new System.Random();

        private TaskCompletionSource<PlayerAction.ActionBase> tcs;

        public void StartGame()
        {
            for (int i = 0; i < 20; i++)
            {
                Deck.Add(new ActionCard()
                {
                    Id = i,
                    Name = "社群+"+(i%2+1),
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
                p.drawActionCard(this, 4);
            }
            Self = Players[0];
        }

        public void DoAction(int playerId, PlayerAction.ActionBase action)
        {
            Player player = Players[playerId];
            action.HandleAction(this, player);
        }

        public void UseCard(int playerId, int CardId, PlayerAction.ActionBase target)
        {
            Player player = Players[playerId];
            player.UseCard(this, CardId, target);
        }

        public void UseSkill(int playerId, int CardId, PlayerAction.ActionBase target)
        {

        }

        public void Answer(PlayerAction.ActionBase target)
        {
            tcs.TrySetResult(target);
        }
        public Task<PlayerAction.ActionBase> WaitAnswer() //where T: Target.TargetBase
        {
            tcs = new TaskCompletionSource<PlayerAction.ActionBase>();
            return tcs.Task;
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
