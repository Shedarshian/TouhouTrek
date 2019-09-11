using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public class Card
    {
        public int Id;
        public CardTypeEnum CardType;

        /// <summary>
        /// 这张牌能用怎么样的方式打出
        /// </summary>
        /// <returns></returns>
        public virtual Type GetUseType()
        {
            return typeof(Target.Simple);
        }

        internal virtual void DoEffect(Game game, Target.TargetBase target)
        {
        }

    }

    public enum CardTypeEnum
    {
        Charactor,
        Theme,
        Event,
        Action,
    }
}
