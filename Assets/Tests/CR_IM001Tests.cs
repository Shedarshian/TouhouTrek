using System.Collections.Generic;
using NUnit.Framework;

using ZMDFQ;
using ZMDFQ.Cards;
using ZMDFQ.PlayerAction;

namespace Tests
{
    public class CR_IM001Tests
    {
        [Test]
        public void skill1Test()
        {
            Game game = new Game();
            game.Init(new GameOptions()
            {
                players = new Player[]
                {
                    new Player(0),
                    new Player(1)
                },
                characterCards = game.createCards(new CR_IM001(), 20),
                actionCards = game.createCards(new TestAction1(), 20),
                officialCards = game.createCards(new TestOfficial() { onEnable = g => g.Size += 1 }, 20),
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
            game.Players[0].Hero.Skills[0].AutoRequest = true;
            game.Answer(new EndFreeUseResponse() { PlayerId = 0 });
            game.Answer(new ChooseDirectionResponse() { PlayerId = 0, CardId = 61, IfForward = true });
            game.Players[0].Size = -5;
            game.Answer(new ChooseSomeCardResponse() { PlayerId = 0, Cards = new List<int>() { 21, 22 } });

            Assert.AreEqual(-5, game.Players[0].Size);

            game.Answer(new TakeChoiceResponse() { PlayerId = 0, Index = 1 });

            Assert.AreEqual(6, game.Players[0].ActionCards.Count);
        }
        [Test]
        public async void skill2Test()
        {
            Game game = new Game();
            game.Init(new GameOptions()
            {
                players = new Player[]
                {
                    new Player(0),
                    new Player(1)
                },
                characterCards = game.createCards(new CR_IM001(), 20),
                actionCards = game.createCards(new TestAction1(), 20),
                officialCards = game.createCards(new TestOfficial() { onEnable = g => g.Size += 1 }, 20),
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

            Assert.AreEqual(1, game.Players[0].HandMax(game));

            await game.Players[0].Hero.FaceUp(game);

            Assert.AreEqual(4, game.Players[0].HandMax(game));
        }
        [Test]
        public void skill3Test()
        {
            Game game = new Game();
            game.Init(new GameOptions()
            {
                players = new Player[]
                {
                    new Player(0),
                    new Player(1)
                },
                characterCards = game.createCards(new CR_IM001(), 20),
                actionCards = game.createCards(new TestAction1(), 20),
                officialCards = game.createCards(new TestOfficial() { onEnable = g => g.Size += 1 }, 20),
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
            game.Players[0].Size = -5;
            game.Players[1].Size = -5;

            game.Answer(new ChooseHeroResponse() { PlayerId = 0, HeroId = 1 });
            game.Answer(new ChooseHeroResponse() { PlayerId = 1, HeroId = 4 });
            game.Answer(new EndFreeUseResponse() { PlayerId = 0 });
            game.Answer(new ChooseDirectionResponse() { PlayerId = 0, CardId = 61, IfSet = true });
            game.Answer(new ChooseSomeCardResponse() { PlayerId = 0, Cards = new List<int>() { 21, 22 } });
            game.Answer(new TakeChoiceResponse() { PlayerId = 0, Index = 0 });

            game.Answer(new EndFreeUseResponse() { PlayerId = 1 });
            game.Answer(new ChooseDirectionResponse() { PlayerId = 1, CardId = 62, IfSet = true });
            game.Answer(new ChooseSomeCardResponse() { PlayerId = 1, Cards = new List<int>() { 23, 24 } });
            game.Answer(new TakeChoiceResponse() { PlayerId = 1, Index = 0 });

            Assert.AreEqual(2, game.winners.Length);
            Assert.AreEqual(10, game.winners[0].point);
            Assert.AreEqual(10, game.winners[1].point);
        }
    }
}
