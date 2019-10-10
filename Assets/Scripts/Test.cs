using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UnityEngine;
using ZMDFQ;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Game game = new Game();
        game.Database = ConfigManager.Instance;
        Player p = new Player(0);
        game.Players.Add(p);
        game.EventSystem = new SeatByEventSystem();
        HeroCard card = ConfigManager.Instance.GetCard(101) as HeroCard;
        card.Init(game, p);
        Debug.Log(MongoHelper.ToJson(card.Skills.Count));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
