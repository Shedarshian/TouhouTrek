using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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

        public List<Card> Cards = new List<Card>();

        private TaskCompletionSource<Target.TargetBase> tcs;

        public void StartGame()
        {
            for (int i = 0; i < 4; i++)
            {
                Players.Add(new Player());
            }
            for (int i = 0; i < 10; i++)
            {
                Cards.Add(new Card()
                {
                    Id = i,
                    Effects = new List<EffectBase>() {
                        new Effect.ChangeMainSize(){ Size=1}
                    }
                });
            }
        }



        public void UseCard(int playerId, int CardId, Target.TargetBase target)
        {
            Player player = Players[playerId];
            Card card = Cards.Find(x => x.Id == CardId);
            card.DoEffect(this, target);
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


        internal void ChangeSize(int size)
        {
            var data = new EventData<int> { data = size };
            EventSystem.Call(EventEnum.OnGameSizeChange, data);
            Size += data.data;
            Debug.Log($"Game size change to {Size}");
        }
    }
}
