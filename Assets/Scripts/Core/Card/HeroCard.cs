using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public class HeroCard : Card
    {
        [BsonIgnore]
        public Player Player { get; private set; }
        /// <summary>
        /// 角色牌是否正面朝上？
        /// </summary>
        [BsonIgnore]
        public bool isFaceup { get; set; } = false;

        /// <summary>
        /// 阵营
        /// </summary>
        public CampEnum camp;
        [BsonIgnore]
        public List<Skill> Skills;

        public List<int> SkillIds;
        public void Init(Game game, Player player)
        {
            Player = player;
            Skills = new List<Skill>();
            if (SkillIds != null)
                foreach (int skillId in SkillIds)
                {
                    Skill skill = game.Database.GetSkill(skillId);
                    skill.Hero = this;
                    Skills.Add(skill);
                    skill.Enable(game);
                }
        }
        public async Task FaceUp(Game game)
        {
            if (!isFaceup)
            {
                isFaceup = true;
                await game.EventSystem.Call(EventEnum.FaceUp,game.ActivePlayerSeat(), game, this);
            }
        }
    }
    /// <summary>
    /// 角色阵营
    /// </summary>
    public enum CampEnum
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
