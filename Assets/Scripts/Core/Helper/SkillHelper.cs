using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public class SkillHelper
    {
        static List<Type> list = new List<Type>();

        static SkillHelper()
        {
            foreach (var type in typeof(Game).Assembly.GetTypes())
            {
                if (typeof(Skill).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    Log.Debug($"获取到技能实例类型:{type.Name},Id:{list.Count - 1}");
                    list.Add(type);
                }
            }
        }
        public static int getId(Skill skill)
        {
            return list.IndexOf(skill.GetType());
        }
        public static Type getType(int id)
        {
            return list[id];
        }
    }
}
