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
        public IEnumerator initTest()
        {
            Game game = new Game();
            game.Init(new GameOptions()
            {
                players = new Player[]
                {
                    new Player(0),
                    new Player(1)
                },
                actionCards = game.createCards(new TestAction1(), 20),
                officialCards = game.createCards(new TestOfficial(), 20),
                eventCards = game.createCards(new TestEvent(), 20),
                shuffle = false,
                initCommunitySize = 0,
                initInfluence = 0,
            });

            Assert.AreEqual(2, game.Players.Count);
            Assert.IsInstanceOf<Player>(game.Players[0]);
            Assert.IsInstanceOf<Player>(game.Players[1]);
            Assert.AreEqual(20, game.Deck.Count);
            Assert.AreEqual(20, game.ThemeDeck.Count);
            Assert.AreEqual(20, game.EventDeck.Count);
            Assert.AreEqual(0, game.Size);
            yield break;
        }
        [UnityTest]
        public IEnumerator startGameTest()
        {
            Game game = new Game();
            game.Init(new GameOptions()
            {
                players = new Player[]
                {
                    new Player(0),
                    new Player(1)
                },
                characterCards = game.createCards(new TestCharacter(), 20),
                actionCards = game.createCards(new TestAction1(), 20),
                officialCards = game.createCards(new TestOfficial(), 20),
                eventCards = game.createCards(new TestEvent(), 20),
                firstPlayer = 0,
                shuffle = false,
                initCommunitySize = 0,
                initInfluence = 0,
                chooseCharacter = true,
                doubleCharacter = false
            });
            game.StartGame();
            yield return new WaitForSeconds(1);//我也假装思考1秒
            game.Answer(new ChooseHeroResponse() { PlayerId = 0, HeroId = 1 });
            game.Answer(new ChooseHeroResponse() { PlayerId = 1, HeroId = 4 });

            Assert.AreEqual(0, game.ActivePlayer.Id);
            Assert.AreEqual(0, game.Players[0].Size);
            Assert.IsInstanceOf<TestCharacter>(game.Players[0].Hero);
            Assert.AreEqual(0, game.Players[1].Size);
            Assert.IsInstanceOf<TestCharacter>(game.Players[1].Hero);
            Assert.IsInstanceOf<TestOfficial>(game.ActiveTheme);
            Assert.AreEqual(1, game.Size);
        }
        class TestCharacter : HeroCard
        {
            public override List<Skill> Skills { get; } = new List<Skill>(new Skill[] { });
        }
        class TestSkill : Skill
        {
            protected override UseWay useWay()
            {
                throw new System.NotImplementedException();
            }

            public override void Disable(Game game)
            {
                throw new System.NotImplementedException();
            }

            public override Task DoEffect(Game game, UseInfo useInfo)
            {
                throw new System.NotImplementedException();
            }

            public override void Enable(Game game)
            {
                throw new System.NotImplementedException();
            }
        }
        class TestAction1 : ActionCard<SimpleRequest, SimpleResponse>
        {
            protected override async Task doEffect(Game game, SimpleResponse useWay)
            {
            }
            protected override SimpleRequest useWay()
            {
                return SimpleRequest.Instance;
            }
        }
        class TestOfficial : ThemeCard
        {
            public override void Enable(Game game)
            {
                game.Size += 1;
            }
            public override void Disable(Game game)
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
