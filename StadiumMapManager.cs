using System;
using BaseMod;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace KazimierzMajor
{
    public class StadiumMapManager: CustomMapManager
    {
        public override void CustomInit()
        {
            base.CustomInit();
            this.Retextualize();
            if (Singleton<StageController>.Instance.CurrentWave <= 3)
                this.mapBgm = new AudioClip[3] { Harmony_Patch.Corrotion, Harmony_Patch.Corrotion, Harmony_Patch.Corrotion };
            else
                this.mapBgm = new AudioClip[3] { Harmony_Patch.Putrid, Harmony_Patch.Putrid, Harmony_Patch.Putrid };
            this.mapSize = MapSize.M;
            this._bMapInitialized = true;
        }
        public override void EnableMap(bool b)
        {
            this.gameObject.SetActive(b);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            SingletonBehavior<BattleSoundManager>.Instance.SetEnemyTheme(this.mapBgm);
        }
        public override GameObject GetWallCrater() => (GameObject)null;
        public override GameObject GetScratch(int lv, Transform parent) => (GameObject)null;
        private void Retextualize()
        {           
            GameObject bg = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
            DuplicateSprite(bg, "background");
            for(int i=1; i<6; i++)
                this.gameObject.transform.GetChild(1).GetChild(0).GetChild(i).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
        }
        private void DuplicateSprite(GameObject obj, string path, float ReactWidth=1, float RectLength=1)
        {
            Texture2D texture2D = new Texture2D(1,1);
            texture2D.LoadImage(File.ReadAllBytes(Harmony_Patch.ModPath + "/ArtWork/"+path+".png"));
            Sprite sprite = obj.GetComponent<SpriteRenderer>().sprite;
            obj.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float)texture2D.width*ReactWidth, (float)texture2D.height* RectLength), new Vector2(0.5f, 0.5f), sprite.pixelsPerUnit, 0U, SpriteMeshType.FullRect);
        }
    }
}
