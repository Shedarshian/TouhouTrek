using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FairyGUI;
using ZMDFQ.UI;

namespace ZMDFQ
{
    public class UI_Battle:MonoBehaviour
    {
        public GComponent _main;
        private void Awake()
        {
            FairyGUIRegister.Register();
        }
        private void Start()
        {
            _main = GetComponent<UIPanel>().ui;
        }

        public void FlushView(Game game)
        {

        }
    }
}
