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

        /// <summary>
        /// 是否处于自由出牌的状态
        /// </summary>
        private bool freeUse;
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

            for (int i = 0; i < 3; i++)
            {
                var ui_hero = _main.GetChild("n28").asCom.GetChild("hero" + i);
                ui_hero.onClick.Add((x) =>
                {
                    Game.Answer(new ChooseHeroResponse()
                    {
                        playerId = Game.Self.Id,
                        HeroId = int.Parse(ui_hero.text),
                    });
                    _main.GetController("ChooseHero").selectedIndex = 0;
                });
            }

            Game = new Game();

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
                    FlushView(Game);
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
                FlushView(Game);
            });


            Game.EventSystem.Register(EventEnum.ActionStart, actionStart);

            Game.EventSystem.Register(EventEnum.ActionEnd, actionEnd);

            Game.EventSystem.Register(EventEnum.DrawActionCard, onDrawCard);

            Game.EventSystem.Register(EventEnum.DropActionCard, onDropCard);

            Game.OnRequest += OnRequest;

            Game.StartGame();

            FlushView(Game);
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
            HandCards.SetCards(game.Self.ActionCards, SelectedCards);

            _main.GetController("State").selectedIndex = 0;

            if (nowRequest != null)
                checkUse(nowRequest);
            else if (freeUse && SelectedCards.Count == 1)
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
                case ChooseSomeCardRequest  dropCardRequest:
                    nowRequest = dropCardRequest;
                    FlushView(Game);
                    break;
                case ChooseHeroRequest chooseHeroRequest:
                    _main.GetController("ChooseHero").selectedIndex = 1;
                    for (int i = 0; i < 3; i++)
                    {
                        _main.GetChild("n28").asCom.GetChild("hero" + i).text = chooseHeroRequest.HeroIds[i].ToString();
                    }
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
                case ChooseSomeCardRequest  dropCardRequest:
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
                case ChooseSomeCardRequest  dropCardRequest:
                    Game.DoAction(new ChooseSomeCardResponse() { playerId = Game.Self.Id, Cards = SelectedCards });
                    break;
            }
            SelectedCards.Clear();
            nowRequest = null;
            FlushView(Game);
        }

        void actionStart(object[] param)
        {
            Game game = param[0] as Game;
            if (game.ActivePlayer == game.Self)
                freeUse = true;
        }

        void actionEnd(object[] param)
        {
            Game game = param[0] as Game;
            if (game.ActivePlayer == game.Self)
                freeUse = false;
        }

        void onDrawCard(object[] param)
        {
            string s = string.Empty;
            s += (param[0] as Player).Id.ToString()+"抽到了";
            foreach (ActionCard card in (param[1] as List<ActionCard>))
            {
                s += card.Name + ",";
            }
            Log.Debug(s);
            FlushView(Game);
        }

        void onDropCard(object[] param)
        {
            string s = string.Empty;
            s += (param[0] as Player).Id.ToString() + "丢掉了";
            foreach (ActionCard card in (param[1] as List<ActionCard>))
            {
                s +=  card.Name + ",";
            }
            Log.Debug(s);
            FlushView(Game);
        }
    }
}
