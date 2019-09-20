using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using ZMDFQ;
using ZMDFQ.Cards;
using ZMDFQ.Effects;
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
            game.Answer(new ChooseHeroResponse() { PlayerId = 0, HeroId = 1 });
            game.Answer(new ChooseHeroResponse() { PlayerId = 1, HeroId = 4 });

            Assert.AreEqual(0, game.ActivePlayer.Id);
            Assert.AreEqual(0, game.Players[0].Size);
            Assert.IsInstanceOf<TestCharacter>(game.Players[0].Hero);
            Assert.AreEqual(0, game.Players[1].Size);
            Assert.IsInstanceOf<TestCharacter>(game.Players[1].Hero);
            Assert.IsInstanceOf<TestOfficial>(game.ActiveTheme);
            Assert.AreEqual(1, game.Size);
            Assert.AreEqual(1, game.ActivePlayer.EventCards.Count);
            Assert.IsInstanceOf<TestEvent>(game.ActivePlayer.EventCards[0]);
            Assert.AreEqual(3, game.ActivePlayer.ActionCards.Count);
            Assert.IsInstanceOf<TestAction1>(game.ActivePlayer.ActionCards[0]);
            Assert.IsInstanceOf<TestAction1>(game.ActivePlayer.ActionCards[1]);
            Assert.IsInstanceOf<TestAction1>(game.ActivePlayer.ActionCards[2]);
            yield break;
        }
        [UnityTest]
        public IEnumerator useActionTest()
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
            game.Answer(new ChooseHeroResponse() { PlayerId = 0, HeroId = 1 });
            game.Answer(new ChooseHeroResponse() { PlayerId = 1, HeroId = 4 });
            game.Answer(new SimpleResponse() { PlayerId = 0, CardId = 21 });

            Assert.AreEqual(1, game.Players[0].Size);
            yield break;
        }
        [UnityTest]
        public IEnumerator useEventTest()
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
            game.Answer(new ChooseHeroResponse() { PlayerId = 0, HeroId = 1 });
            game.Answer(new ChooseHeroResponse() { PlayerId = 1, HeroId = 4 });
            game.Answer(new SimpleResponse() { PlayerId = 0, CardId = 21 });
            game.Answer(new SimpleResponse() { PlayerId = 0, CardId = 22 });
            game.Answer(new EndTurnResponse() { PlayerId = 0 });//TODO:回合结束的设定也许需要修改，按道理来讲应该先进入事件结算。
            game.Answer(new ChooseDirectionResponse() { PlayerId = 0, CardId = 61, IfForward = true });

            Assert.AreEqual(4, game.Players[0].Size);
            yield break;
        }
        [UnityTest]
        public IEnumerator useEventReverseTest()
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
            game.Answer(new ChooseHeroResponse() { PlayerId = 0, HeroId = 1 });
            game.Answer(new ChooseHeroResponse() { PlayerId = 1, HeroId = 4 });
            game.Answer(new SimpleResponse() { PlayerId = 0, CardId = 21 });
            game.Answer(new SimpleResponse() { PlayerId = 0, CardId = 22 });
            game.Answer(new EndTurnResponse() { PlayerId = 0 });//TODO:回合结束的设定也许需要修改，按道理来讲应该先进入事件结算。
            game.Answer(new ChooseDirectionResponse() { PlayerId = 0, CardId = 61, IfForward = false });

            Assert.AreEqual(0, game.Players[0].Size);
            yield break;
        }
        [UnityTest]
        public IEnumerator setEventTest()
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
            game.Answer(new ChooseHeroResponse() { PlayerId = 0, HeroId = 1 });
            game.Answer(new ChooseHeroResponse() { PlayerId = 1, HeroId = 4 });
            game.Answer(new EndTurnResponse() { PlayerId = 0 });//TODO:回合结束的设定也许需要修改，按道理来讲应该先进入事件结算。
            game.Answer(new ChooseDirectionResponse() { PlayerId = 0, CardId = 61, IfSet = true });
            game.Answer(new ChooseSomeCardResponse() { PlayerId = 0, Cards = new List<int>() { 21, 22 } });

            Assert.IsInstanceOf<TestEvent>(game.Players[0].SaveEvent);
            Assert.AreEqual(61, game.Players[0].SaveEvent.Id);

            game.Answer(new EndTurnResponse() { PlayerId = 1 });
            game.Answer(new ChooseDirectionResponse() { PlayerId = 1, CardId = 62, IfSet = true });
            game.Answer(new ChooseSomeCardResponse() { PlayerId = 1, Cards = new List<int>() { 23, 24 } });

            Assert.IsInstanceOf<TestEvent>(game.Players[1].SaveEvent);
            Assert.AreEqual(62, game.Players[1].SaveEvent.Id);

            game.Answer(new SimpleResponse() { PlayerId = 0, CardId = 25 });
            game.Answer(new EndTurnResponse() { PlayerId = 0 });
            game.Answer(new ChooseDirectionResponse() { PlayerId = 0, CardId = 63, IfSet = true });

            Assert.AreEqual(2, game.Players[0].Size);
            Assert.IsInstanceOf<TestEvent>(game.Players[0].SaveEvent);
            Assert.AreEqual(63, game.Players[0].SaveEvent.Id);

            yield break;
        }
        [UnityTest]
        public IEnumerator discardTest()
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
            game.Answer(new ChooseHeroResponse() { PlayerId = 0, HeroId = 1 });
            game.Answer(new ChooseHeroResponse() { PlayerId = 1, HeroId = 4 });
            game.Answer(new SimpleResponse() { PlayerId = 0, CardId = 21 });
            game.Answer(new EndTurnResponse() { PlayerId = 0 });//TODO:回合结束的设定也许需要修改，按道理来讲应该先进入事件结算。
            game.Answer(new ChooseDirectionResponse() { PlayerId = 0, CardId = 61, IfForward = true });

            Assert.AreEqual(2, game.Players[0].Size);
            Assert.AreEqual(22, game.Players[0].ActionCards[0].Id);
            Assert.AreEqual(25, game.Players[0].ActionCards[1].Id);
            Assert.AreEqual(2, game.Players[0].ActionCards.Count);

            game.Answer(new EndTurnResponse() { PlayerId = 1 });
            game.Answer(new ChooseDirectionResponse() { PlayerId = 1, CardId = 62, IfSet = true });
            game.Answer(new ChooseSomeCardResponse() { PlayerId = 1, Cards = new List<int>() { 23, 24 } });

            Assert.AreEqual(1, game.Players[1].ActionCards.Count);
            Assert.AreEqual(21, game.UsedActionDeck[0].Id);
            Assert.AreEqual(23, game.UsedActionDeck[1].Id);
            Assert.AreEqual(24, game.UsedActionDeck[2].Id);
            Assert.AreEqual(3, game.UsedActionDeck.Count);
            yield break;
        }
        [UnityTest]
        public IEnumerator winTest()
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
                doubleCharacter = false,
                endingOfficialCardCount = 1
            });
            game.StartGame();
            game.Answer(new ChooseHeroResponse() { PlayerId = 0, HeroId = 1 });
            game.Answer(new ChooseHeroResponse() { PlayerId = 1, HeroId = 4 });
            game.Answer(new SimpleResponse() { PlayerId = 0, CardId = 21 });
            game.Answer(new EndTurnResponse() { PlayerId = 0 });//TODO:回合结束的设定也许需要修改，按道理来讲应该先进入事件结算。
            game.Answer(new ChooseDirectionResponse() { PlayerId = 0, CardId = 61, IfForward = true });

            game.Answer(new EndTurnResponse() { PlayerId = 1 });
            game.Answer(new ChooseDirectionResponse() { PlayerId = 1, CardId = 62, IfSet = true });
            game.Answer(new ChooseSomeCardResponse() { PlayerId = 1, Cards = new List<int>() { 23, 24 } });

            Assert.AreEqual(game.Players[0], game.winner);
            yield break;
        }
        class TestCharacter : HeroCard
        {
            public override Camp camp
            {
                get { return Camp.commuMajor; }
            }
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
            protected override Task doEffect(Game game, SimpleResponse useWay)
            {
                return UseCard.NormalUse(game, useWay, this, (g, r) =>
                {
                    game.Players.Find(p => p.Id == useWay.PlayerId).Size += 1;
                    return Task.CompletedTask;
                });
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
            public override Task UseForward(Game game, Player user)
            {
                user.Size *= 2;
                return Task.CompletedTask;
            }
            public override Task UseBackward(Game game, Player user)
            {
                user.Size = 0;
                return Task.CompletedTask;
            }
        }
    }
}
