using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;

namespace ZMDFQ
{
    public class ResHelper
    {
        static Dictionary<string, SpriteAtlas> AtlasDic = new Dictionary<string, SpriteAtlas>();

        public static TextAsset GetData(string fileName)
        {
            return ResourcesManager.Instance.GetAsset<TextAsset>(PathHelper.DataPath + fileName, fileName);
        }

        public static Texture GetStandPicture(string PictureName)
        {
            return ResourcesManager.Instance.GetAsset<Texture>(PathHelper.StandPicturePath + PictureName, PictureName);
        }

        public static Sprite GetSprite(string atlaName,string spriteName)
        {
            SpriteAtlas spriteAtlas;
            AtlasDic.TryGetValue(atlaName, out spriteAtlas);
            if (spriteAtlas == null)
            {
                spriteAtlas = ResourcesManager.Instance.GetAsset<SpriteAtlas>(PathHelper.SpritesPath + atlaName, atlaName);
                AtlasDic.Add(atlaName, spriteAtlas);
            }
            return spriteAtlas.GetSprite(spriteName);
        }
    }
}
