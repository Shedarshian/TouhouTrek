using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public abstract class HeroCard : Card
    {
        public Player Player { get; private set; }
        /// <summary>
        /// 角色牌是否正面朝上？
        /// </summary>
        public bool isFaceup { get; set; } = false;
        public void Init(Game game, Player player)
        {
            Player = player;
            foreach (var skill in Skills)
            {
                skill.Hero = this;
                skill.Enable(game);
            }
        }
        /// <summary>
        /// 阵营
        /// </summary>
        public abstract Camp camp { get; }
        public abstract List<Skill> Skills { get; }

        public async Task FaceUp(Game game)
        {
            if (!isFaceup)
            {
                isFaceup = true;
                await game.EventSystem.Call(EventEnum.FaceUp, game, this);
            }
        }
    }
    /// <summary>
    /// 角色阵营
    /// </summary>
    public enum Camp
    {
        /// <summary>
        /// 社群繁荣
        /// </summary>
        commuMajor,
        /// <summary>
        /// 个人繁荣
        /// </summary>
        indivMajor,
        /// <summary>
        /// 社群小众
        /// </summary>
        commuMinor,
        /// <summary>
        /// 个人小众
        /// </summary>
        indivMinor,
        /// <summary>
        /// 中立
        /// </summary>
        neutral = 4
    }
}
