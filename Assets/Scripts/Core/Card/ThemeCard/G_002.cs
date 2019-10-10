using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 东方红魔乡：无法使用或者发动效果中包含判定动作的行动牌和技能。
    /// </summary>
    public class G_002 : ThemeCard
    {
        public override string Name => "东方红魔乡";
        List<Type> limitType = new List<Type>()
        {

        };
        public override void Enable(Game game)
        {
            game.EventSystem.Register(EventEnum.onCheckCanUse, -1, changeUseable);
        }

        public override void Disable(Game game)
        {
            game.EventSystem.Remove(EventEnum.onCheckCanUse, changeUseable);
        }

        Task changeUseable(object[] args)
        {
            if (args[0] is IDiceCheck)
            {
                EventData<bool> booldata = args[1] as EventData<bool>;
                booldata.data = false;
            }
            return Task.CompletedTask;
        }
    }
}
