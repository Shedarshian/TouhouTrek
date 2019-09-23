using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public abstract class HeroCard : Card
    {
        Player Player;
        public void Init(Player player)
        {
            this.Player = player;
            foreach (var skill in Skills)
            {
                skill.Hero = this;
            }
        }
        /// <summary>
        /// 阵营
        /// </summary>
        public abstract Camp camp { get; }
        public abstract List<Skill> Skills { get; }
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
