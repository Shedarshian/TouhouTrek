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

        public List<Card> Deck = new List<Card>();

        public List<Card> UsedDeck = new List<Card>();

        public List<Card> WeatherDeck = new List<Card>();

        public List<Card> UsedWeatherDeck = new List<Card>();

        public List<Card> EventDeck = new List<Card>();

        public List<Card> UsedEventDeck = new List<Card>();

        public Player Self;

        private System.Random ram = new System.Random();

        private TaskCompletionSource<Target.TargetBase> tcs;

        public void StartGame()
        {
            for (int i = 0; i < 8; i++)
            {
                Players.Add(new Player());
            }
            Self = Players[0];
            for (int i = 0; i < 10; i++)
            {
                Deck.Add(new ActionCard()
                {
                    Id = i,
                    Effects = new List<EffectBase>() {
                        new Effect.ChangeMainSize(){ Size=1
                        }
                    }
                });
            }
        }



        public void UseCard(int playerId, int CardId, Target.TargetBase target)
        {
            Player player = Players[playerId];
            player.UseCard(this, CardId, target);
        }

        public void UseSkill(int playerId, int CardId, Target.TargetBase target)
        {

        }

        public void Answer(Target.TargetBase target)
        {
            tcs.TrySetResult(target);
        }
        public Task<Target.TargetBase> WaitAnswer() //where T: Target.TargetBase
        {
            tcs = new TaskCompletionSource<Target.TargetBase>();
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
