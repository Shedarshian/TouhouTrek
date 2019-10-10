﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ZMDFQ
{
    public class ConfigManager : IDatabase
    {
        public static ConfigManager Instance
        {
            get
            {
                return instance == null ? instance = new ConfigManager().Init() : instance;
            }
        }
        private static ConfigManager instance;
        public Dictionary<int, Card> Cards;
        public Dictionary<int, DeckConfig> Decks;
        public ConfigManager Init()
        {
            load("Card", out Cards, (x) => x.ConfigId);
            load("Deck", out Decks, (x) => x.ConfigId);
            return this;
        }

        void load<T>(string path, out Dictionary<int,T> data,Func<T,int> getId)
        {
            string text = ResHelper.GetData(path).text;
            string[] jsons = text.Split('\n');
            data = new Dictionary<int, T>();
            for (int i = 0; i < jsons.Length; i++)
            {
                if (string.IsNullOrEmpty(jsons[i])) continue;
                T t = MongoHelper.FromJson<T>(jsons[i]);
                data.Add(getId(t), t);
            }
        }

        public Card Get(int configId)
        {
            Card card;
            if (Cards.TryGetValue(configId, out card))
            {
                return MongoHelper.Clone(Cards[configId]);
            }
            else
            {
                return null;
            }
        }

        public T GetCard<T>() where T : Card
        {
            return MongoHelper.Clone(Cards.FirstOrDefault(x => x.Value is T) as T);
        }

        public GameOptions GetGameOption(string gameType,GameOptions.PlayerInfo[] playerInfos)
        {
            var deck = Decks.FirstOrDefault(x => x.Value.Mode == gameType).Value;
            if (deck == null) return null;
            GameOptions gameOptions = new GameOptions()
            {
                Database = this,
                PlayerInfos = playerInfos,
                firstPlayer = 0,
                shuffle = true,
                initCommunitySize = 0,
                initInfluence = 0,
                chooseCharacter = true,
                doubleCharacter = false,
                endingOfficialCardCount = 1
            };
            var cards= new List<int>();
            gameOptions.Cards = cards;
            for (int i = 0; i < deck.CardTypes.Length; i++)
            {
                if (Get(deck.CardTypes[i]) == null) continue;
                for (int j = 0; j < deck.CardNumbers[i]; j++)
                {
                    cards.Add(deck.CardTypes[i]);
                }
                Log.Debug($"add:{deck.CardTypes[i]},count:{deck.CardNumbers[i]}");
            }
            return gameOptions;
        }
    }

    public class DeckConfig
    {
        public string Mode;
        public int ConfigId;
        public int[] CardTypes;
        public int[] CardNumbers;
    }
}
