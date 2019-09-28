using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    /// <summary>
    /// 自由出牌时的一次行动
    /// </summary>
    public class FreeUse : Response
    {
        /// <summary>
        /// 大于0时表示是正常用卡
        /// </summary>
        public int CardId = -1;

        /// <summary>
        /// 用卡时选中的角色目标
        /// </summary>
        public List<int> PlayersId = new List<int>();

        /// <summary>
        /// 使用技能时，可能一次需要选中多张手牌
        /// </summary>
        public List<int> Source;

        /// <summary>
        /// 大于0时表示使用了技能
        /// </summary>
        public int SkillId = -1;

        public Task HandleAction(Game game)
        {
            return game.GetPlayer(PlayerId).UseActionCard(game, this);
        }
    }
}
