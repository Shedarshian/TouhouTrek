using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using FairyGUI.Utils;

namespace ZMDFQ.UI
{
    public class UI_Player:GButton
    {
        public const string URL = "ui://oacz4rtmqfxq2";
        public Player Player;
        public static UI_Player CreateInstance()
        {
            return (UI_Player)UIPackage.CreateObject("Battle", "PlayerInfo");
        }
        public void SetPlayerCard(Player player)
        {
            this.Player = player;
            GetChild("Cards").text = player.Cards.Count.ToString();
            GetChild("Size").text = player.Size.ToString();
            GetChild("Name").text = player.Hero.Name;
        }
    }
}
