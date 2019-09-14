using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FairyGUI;
using ZMDFQ.UI;
using ZMDFQ.PlayerAction;

namespace ZMDFQ
{
    public class UI_Battle:MonoBehaviour
    {
        public GComponent _main;
        public Game Game;
        public List<UI_Player> SelectedPlayers = new List<UI_Player>();
        public List<ActionCard> SelectedCards = new List<ActionCard>();
        public UI_Hands HandCards;

        private Request nowRequest;


        private void Awake()
        {
            FairyGUIRegister.Register();
        }
        private void Start()
        {
            _main = GetComponent<UIPanel>().ui;
            HandCards = _main.GetChild("HandCards") as UI_Hands;

            _main.GetChild("n17").onClick.Add(takeUse);

            _main.GetChild("EndTurn").onClick.Add(() => Game.DoAction(new EndTurn() { playerId = Game.Self.Id }));

            Game game = new Game();

            for (int i = 0; i < 8; i++)
            {
                var ui_player = _main.GetChild("Player" + i) as UI_Player;
                ui_player.changeStateOnClick = false;
                ui_player.onClick.Add(() =>
                {
                    if (nowRequest is ChooseSomeoneRequest request && request.Number > 1)
                    {
                        //复选玩家模式
                        if (SelectedPlayers.Contains(ui_player))
                            SelectedPlayers.Remove(ui_player);
                        else
                            SelectedPlayers.Add(ui_player);
                    }
                    else
                    {
                        if (SelectedPlayers.Count == 0) SelectedPlayers.Add(ui_player);
                        else if (SelectedPlayers[0] == ui_player) SelectedPlayers.Clear();
                        else SelectedPlayers[0] = ui_player;
                    }
                    FlushView(game);
                });
            }
            HandCards.OnCardClick.Add((x) =>
            {
                var ui_card = x.data as UI_ActionCard;
                if (SelectedCards.Contains(ui_card.Card))
                {
                    SelectedCards.Remove(ui_card.Card);
                }
                else
                {
                    SelectedCards.Add(ui_card.Card);
                }
                FlushView(game);
            });

            game.StartGame();

            game.EventSystem.Register(EventEnum.TurnStart, turnStart);

            game.OnRequest += OnRequest;

            FlushView(game);
        }

        public void FlushView(Game game)
        {
            this.Game = game;
            int playerIndex = game.Players.IndexOf(game.Self);
            for (int i = 0; i < game.Players.Count; i++)
            {
                var ui_player = _main.GetChild("Player" + (7 - i)) as UI_Player;
                ui_player.SetPlayerCard(game.Players[i]);
                ui_player.selected = SelectedPlayers.Contains(ui_player);
            }
            HandCards.SetCards(game.Self.Cards, SelectedCards);

            _main.GetController("State").selectedIndex = 0;

            if (SelectedCards.Count == 1)
            {
                checkUse(SelectedCards[0].RequestWay);
            }

            _main.GetChild("Deck").text = game.Deck.Count.ToString();
            _main.GetChild("Size").text = game.Size.ToString();
        }

        void OnRequest(Game game,Request request)
        {
            if (request.playerId != game.Self.Id) return;
            switch (request)
            {
                case DropCardRequest dropCardRequest:
                    nowRequest = dropCardRequest;
                    FlushView(Game);
                    break;
            }
        }

        void checkUse(Request request)
        {
            Controller c = _main.GetController("State");

            nowRequest = request;

            switch (request)
            {
                case SimpleRequest simpleRequest:
                    c.selectedIndex = 1;
                    break;
                case ChooseSomeoneRequest chooseSomeoneRequest:
                    c.selectedIndex = SelectedPlayers.Count == chooseSomeoneRequest.Number ? 1 : 0;
                    break;
                case ChooseDirectionRequest chooseDirectionRequest:
                    break;
                case DropCardRequest dropCardRequest:
                    c.selectedIndex = SelectedCards.Count == dropCardRequest.Count ? 1 : 0;
                    break;
            }
        }

        void takeUse()
        {            
            switch (nowRequest)
            {
                case SimpleRequest simpleRequest:
                    Game.DoAction(new SimpleResponse() { playerId = Game.Self.Id, CardId = SelectedCards[0].Id });
                    break;
                case ChooseSomeoneRequest chooseSomeoneRequest:
                    Game.DoAction(new ChooseSomeoneResponse() { playerId = Game.Self.Id, Targets = SelectedPlayers.Select(x => x.Player).ToList() });
                    break;
                case ChooseDirectionRequest chooseDirectionRequest:
                    break;
                case DropCardRequest dropCardRequest:
                    Game.DoAction(new DropCardResponse() { playerId = Game.Self.Id, Cards = SelectedCards });
                    break;
            }
            SelectedCards.Clear();
            nowRequest = null;
            FlushView(Game);
        }

        void turnStart(object[] param)
        {
            //freeUse = true;
        }
    }
}
