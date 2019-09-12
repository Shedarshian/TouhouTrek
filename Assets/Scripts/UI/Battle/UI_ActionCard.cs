using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FairyGUI;
using FairyGUI.Utils;

namespace ZMDFQ.UI
{
    public class UI_ActionCard : GComponent
    {
        public ActionCard Card;

        public const string URL = "ui://oacz4rtmqfxq1";
        public static UI_ActionCard CreateInstance()
        {
            return (UI_ActionCard)UIPackage.CreateObject("Battle", "HandCard");
        }
        public void SetCard(ActionCard actionCard)
        {
            this.Card = actionCard;
            GetChild("title").text = actionCard.Name;
        }
    }
}
