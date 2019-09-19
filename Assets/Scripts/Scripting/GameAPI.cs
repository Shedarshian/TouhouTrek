
using ZMDFQ.PlayerAction;

namespace ZMDFQ
{
    /// <summary>
    /// 游戏脚本API，用于为脚本语言提供API
    /// </summary>
    public class GameAPI
    {
        Game game { get; } = null;
        Response target { get; } = null;
        /// <summary>
        /// 触发事件玩家
        /// </summary>
        public Player triggerPlayer
        {
            get { return game.Players.Find(p => { return p.Id == target.PlayerId; }); }
        }
        public GameAPI(Game game, Response target)
        {
            this.game = game;
            this.target = target;
        }
        /// <summary>
        /// 增加玩家影响力，当增加的值为负数的时候则会减少。
        /// </summary>
        /// <param name="player">增加影响力的玩家</param>
        /// <param name="value">增加的值</param>
        public void addPlayerInfluence(Player player, int value)
        {
            player.Size += value;
        }
    }
}