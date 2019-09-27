using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.UI.Battle
{
    using PlayerAction;
    public partial class UI_Main2
    {
        [BattleUI(nameof(onRequest))]
        private void handleRequest()
        {
            if (nowRequest.PlayerId==self.Id)
                switch (nowRequest)
                {
                    case ChooseHeroRequest chooseHeroRequest:
                        //由ChooseHero模块处理
                        break;
                    case FreeUseRequest freeUseRequest:
                        //由DisplayCard处理
                        break;
                    default:
                        Log.Warning($"未处理的request:{nowRequest.GetType().Name}");
                        break;
                }
        }
    }
}
