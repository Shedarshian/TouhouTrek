using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FairyGUI;
using FairyGUI.Utils;

namespace ZMDFQ.UI
{
    public class UI_Card : GComponent
    {
        public const string URL = "ui://oacz4rtmqfxq2";
        public static UI_Card CreateInstance()
        {
            return (UI_Card)UIPackage.CreateObject("Battle", "HandCard");
        }
    }
}
