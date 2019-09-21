using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    /// <summary>
    /// 表示可以将一张卡视为另一张
    /// </summary>
    public interface ITreatAs
    {
        Card TreatTo(Game game, PlayerAction.FreeUse useOneCard);
        bool CanTreat(Game game, PlayerAction.FreeUse useOneCard);
    }
    public class TreatAttribute : Attribute
    {
        public int Id;
    }
    public static class TreatHelper
    {
        static List<Type> list = new List<Type>();

        static TreatHelper()
        {
            foreach (var type in typeof(Game).Assembly.GetTypes())
            {
                if (typeof(ITreatAs).IsAssignableFrom(type))
                {
                    Log.Debug($"获取到treatas{type.Name},Id:{list.Count - 1}");
                    list.Add(type);
                }
            }
        }
        public static int getId(ITreatAs treatAs)
        {
            return list.IndexOf(treatAs.GetType());
        }
        public static Type getType(int id)
        {
            return list[id];
        }
    }
}
