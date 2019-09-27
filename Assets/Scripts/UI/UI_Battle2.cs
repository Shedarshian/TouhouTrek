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
        private void Awake()
        {
            BattleBinder.BindAll();
        }
        // Start is called before the first frame update
        void Start()
        {
            _Main2 = GetComponent<UIPanel>().ui as UI_Main2;
            Game game = new Game();
            game.TimeManager = gameObject.AddComponent<RequestTimeoutManager>();
            game.Init();
            _Main2.SetGame(game, game.GetPlayer(1));
            game.StartGame();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}