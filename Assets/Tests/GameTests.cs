using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using ZMDFQ;
using ZMDFQ.Cards;
using ZMDFQ.PlayerAction;

namespace Tests
{
    public class GameTests
    {
        [UnityTest]
        public IEnumerator startGameTest()
        {
            Game game = new Game();
            game.StartGame(new GameOptions()
            {
                players = new Player[]
                {
                    new Player(0,new TestCharacter()),
                    new AI(game,1,new TestCharacter())
                },
                actionCards = game.createCards(new TestAction1(), 20),
                officialCards = game.createCards(new TestOfficial(), 20),
                eventCards = game.createCards(new TestEvent(), 20),
                shuffle = false,
                initCommunitySize = 0,
                initInfluence = 0,
                chooseCharacter = false,
                doubleCharacter = false
            });
            yield break;
        }
        class TestCharacter : HeroCard
        {
        }
        class TestAction1 : ActionCard<SimpleRequest, SimpleResponse>
        {
            protected override async Task doEffect(Game game, SimpleResponse useWay)
            {
                Effects.GoUsedDeck(game, this, useWay);
            }
            protected override SimpleRequest useWay()
            {
                return SimpleRequest.Instance;
            }
        }
        class TestOfficial : ThemeCard
        {
            public override void Disable(Game game)
            {
            }
            public override void Enable(Game game)
            {
            }
        }
        class TestEvent : EventCard
        {
            public override bool ForwardOnly => false;

            public override Task UseBackward(Game game, Player user)
            {
                return Task.CompletedTask;
            }

            public override Task UseForward(Game game, Player user)
            {
                return Task.CompletedTask;
            }
        }
    }
}
