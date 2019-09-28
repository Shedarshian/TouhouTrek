using System.Collections.Generic;
using NUnit.Framework;

using ZMDFQ;
using ZMDFQ.Cards;
using ZMDFQ.PlayerAction;

namespace Tests
{
    public class CR_CM001Tests
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
                characterCards = game.createCards(new CR_CM001(), 20),
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
            game.setDice(6);
            game.Answer(new FreeUse() { PlayerId = 0, SkillId = SkillHelper.getId(game.Players[0].Hero.Skills[0]), Source = new List<int>() { 21 }, PlayersId = new List<int>() { 1 } });
            game.Answer(new TakeChoiceResponse() { PlayerId = 0, Index = 1 });

            Assert.AreEqual(-2, game.Players[1].Size);
            Assert.AreEqual(2, game.Size);
        }
    }
}
