﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace ZMDFQ
{
    using UI.Battle;
    public class UI_Battle2 : MonoBehaviour
    {
        UI_Main2 _Main2;
        Game game;
        private void Awake()
        {
            BattleBinder.BindAll();
        }
        // Start is called before the first frame update
        void Start()
        {
            LoadPackge("Battle");
            _Main2 = UIPackage.CreateObject("Battle", "Main2") as UI_Main2;
            GRoot.inst.AddChild(_Main2);

            GameOptions.PlayerInfo[] infos = new GameOptions.PlayerInfo[8];
            for (int i = 0; i < infos.Length; i++)
            {
                infos[i] = new GameOptions.PlayerInfo()
                {
                    Id = i == 1 ? i : (-i - 1),
                    Name = "玩家" + i,
                };
            }
            game = new Game();
            game.Database = ConfigManager.Instance;
            game.TimeManager = gameObject.AddComponent<RequestTimeoutManager>();
            //game.Init();
            game.Init(ConfigManager.Instance.GetGameOption("Test",infos));
            _Main2.SetGame(game, game.GetPlayer(1));
            game.StartGame();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            game.CancelGame();
        }

        public void LoadPackge(string PackageName)
        {
#if UNITY_EDITOR
            UIPackage.AddPackage("Assets/" + PathHelper.UIPath + PackageName);
#else
            string p = Path.Combine(PathHelper.AppHotfixResPath, PathHelper.UIPath, PackageName.ToLower());
            AssetBundle res = null;
            string resPath = p + "res";
            if (File.Exists(resPath))
                res = AssetBundle.LoadFromFile(resPath);
            var desc = AssetBundle.LoadFromFile(p + "desc");
            if (res != null)
                UIPackage.AddPackage(desc, res);
            else
            {
                UIPackage.AddPackage(desc);
            }
#endif
        }
    }

}