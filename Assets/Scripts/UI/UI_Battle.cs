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
        public UI_Player SelectedPlayer;
        public List<ActionCard> SelectedCards = new List<ActionCard>();
        public UI_Hands HandCards;

        private Type nowCheckUse;
        private void Awake()
        {
            FairyGUIRegister.Register();
        }
        private void Start()
        {
            _main = GetComponent<UIPanel>().ui;
            HandCards = _main.GetChild("HandCards") as UI_Hands;

            _main.GetChild("n17").onClick.Add(takeUse);

            Game game = new Game();

            for (int i = 0; i < 8; i++)
            {
                var ui_player = _main.GetChild("Player" + i) as UI_Player;
                ui_player.changeStateOnClick = false;
                ui_player.onClick.Add(() =>
                {
                    SelectedPlayer = SelectedPlayer == ui_player ? null : ui_player;
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
                ui_player.selected = SelectedPlayer == ui_player;
            }
            HandCards.SetCards(game.Self.Cards, SelectedCards);

            _main.GetController("State").selectedIndex = 0;
            if (SelectedCards.Count == 1)
            {
                checkUse(SelectedCards[0].GetUseType());
            }

            _main.GetChild("Deck").text = game.Deck.Count.ToString();
            _main.GetChild("Size").text = game.Size.ToString();
        }

        void checkUse(Type usetype)
        {
            Controller c = _main.GetController("State");

            nowCheckUse = usetype;

            if (usetype == typeof(Simple))
            {
                c.selectedIndex = 1;
            }
            if (usetype == typeof(ChooseOneUseCard))
            {
                c.selectedIndex = SelectedPlayer == null ? 0 : 1;
            }
            if (usetype == typeof(ChooseDirection))
            {
                c.selectedIndex = 1;
            }
        }

        void takeUse()
        {
            var usetype = nowCheckUse;
            if (usetype == typeof(Simple))
            {
                Game.UseCard(Game.Self.Id, SelectedCards[0].Id, new PlayerAction.Simple() { CardId = SelectedCards[0].Id });
                SelectedCards.Clear();
            }
            if (usetype == typeof(ChooseOneUseCard))
            {
                Game.UseCard(Game.Self.Id, SelectedCards[0].Id, new PlayerAction.ChooseOneUseCard() { Target = SelectedPlayer.Player });
                SelectedCards.Clear();
            }
            if (usetype == typeof(ChooseDirection))
            {

            }
            FlushView(Game);
        }
    }
}
