using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;

namespace ZMDFQ.UI
{
    public class FairyGUIRegister
    {
        public static void Register()
        {
            UIObjectFactory.SetPackageItemExtension(UI_Player.URL, typeof(UI_Player));
            UIObjectFactory.SetPackageItemExtension(UI_ActionCard.URL, typeof(UI_ActionCard));
            UIObjectFactory.SetPackageItemExtension(UI_Hands.URL, typeof(UI_Hands));
        }
    }
}
