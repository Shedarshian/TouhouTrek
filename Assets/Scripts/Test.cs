using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMDFQ;

public class Test : MonoBehaviour
{
    Game game;
    // Start is called before the first frame update
    void Start()
    {
        game = new Game();
        game.StartGame();

        ThemeCard card = new ThemeCard()
        {
            Effects = new List<EffectBase>()
                {
                    new ZMDFQ.Effect.MoreSizeChange()
                    {
                        Change=1
                    }
                }
        };
        card.DoEffect(game, new ZMDFQ.Target.Simple());

        game.UseCard(0, 0, new ZMDFQ.Target.Simple());

        card.Disable(game);

        game.UseCard(0, 0, new ZMDFQ.Target.Simple());

        Card card1 = new ActionCard()
        {
            Effects = new List<EffectBase>()
            {
                new ZMDFQ.Effect.TestChoice()
                {

                }
            }
        };
        card1.DoEffect(game, new ZMDFQ.Target.Simple());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            game.Answer(new ZMDFQ.Target.Simple());
        }
    }
}
