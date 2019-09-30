using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ZMDFQ
{
    public class ConfigManager
    {
        public static ConfigManager Instance
        {
            get
            {
                return instance == null ? instance = new ConfigManager() : instance;
            }
        }
        private static ConfigManager instance;
        public Card[] Cards;
        public void Init()
        {
            string text = ResHelper.GetData("CardConfig").text;
            string[] jsons = text.Split('\n');
            Cards = new Card[jsons.Length];
            for (int i = 0; i < jsons.Length; i++)
            {
                Cards[i] = MongoHelper.FromJson<Card>(jsons[i]);
            }
        }

        public Card GetCard(int id)
        {
            return MongoHelper.Clone(Cards.FirstOrDefault(x => x.Id == id));
        }

        public T GetCard<T>() where T : Card
        {
            return MongoHelper.Clone(Cards.FirstOrDefault(x => x is T) as T);
        }
    }
}
