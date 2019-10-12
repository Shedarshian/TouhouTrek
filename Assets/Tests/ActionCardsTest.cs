using System.Collections.Generic;

using NUnit.Framework;

using ZMDFQ;
using ZMDFQ.Cards;
using ZMDFQ.PlayerAction;

namespace Tests
{
    public class ActionCardsTest
    {
        [Test]
        public void AT_N003Test()
        {
            Game game = new Game();
            (game.Database as ConfigManager).Cards.Add(0xA000, new TestAction_Empty());
            (game.Database as ConfigManager).Cards.Add(0xC000, new TestCharacter_Empty());
            (game.Database as ConfigManager).Cards.Add(0xF000, new TestOfficial_Empty());
            (game.Database as ConfigManager).Cards.Add(0xE000, new TestEvent_Empty());
            game.Init(new GameOptions()
            {
                PlayerInfos = new GameOptions.PlayerInfo[]
                {
                    new GameOptions.PlayerInfo() { Id = 0 },
                    new GameOptions.PlayerInfo() { Id = 1 }
                },
                Cards = (new int[] { })
                .concatRepeat(game.getCardID<AT_N003>(), 20)//行动
                .concatRepeat(0xC000, 20)//角色
                .concatRepeat(0xF000, 20)//官作
                .concatRepeat(0xE000, 20),//事件
                firstPlayer = 0,
                shuffle = false,
                initCommunitySize = 0,
                initInfluence = 0,
                chooseCharacter = true,
                doubleCharacter = false
            });
            game.StartGame();
            game.Answer(new ChooseHeroResponse() { PlayerId = 0, HeroId = 21 });
            game.Answer(new ChooseHeroResponse() { PlayerId = 1, HeroId = 24 });
            game.Answer(new FreeUse() { PlayerId = 0, CardId = 1, Source = new List<int>() { 1 } });

            Assert.AreEqual(4, game.Players[0].ActionCards.Count);
        }
    }
}
