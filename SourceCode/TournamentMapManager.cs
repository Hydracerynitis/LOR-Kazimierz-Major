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
    public class TournamentMapManager: CustomMapManager
    {
        public override bool IsMapChangable()
        {
            return false;
        }
        public override void CustomInit()
        {
            Retextualize();
            string name = "Vorarephilia";
            LorId stageid = Singleton<StageController>.Instance.GetStageModel().ClassInfo.id;
            if (stageid == Tools.MakeLorId(21600013))
                name = "Champion";
            if (stageid == Tools.MakeLorId(21600043))
                name = "Knight";
            if (stageid == Tools.MakeLorId(21600053))
                name = "Amon";
            AudioClip bgm = KazimierInitializer.BGM[name];
            mapBgm = new AudioClip[3] { bgm,bgm, bgm };
            mapSize = MapSize.L;
            _bMapInitialized = true;
        }
        public override void EnableMap(bool b)
        {
            this.gameObject.SetActive(b);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            SingletonBehavior<BattleSoundManager>.Instance.SetEnemyTheme(mapBgm);
        }
        public override GameObject GetWallCrater() => (GameObject)null;
        public override GameObject GetScratch(int lv, Transform parent) => (GameObject)null;
        private void Retextualize()
        {           
            GameObject bg = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
            DuplicateSprite(bg, "Tournament");
            for (int i=1; i<6; i++)
                this.gameObject.transform.GetChild(1).GetChild(0).GetChild(i).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
        }
        private void DuplicateSprite(GameObject obj, string path, float ReactWidth=1, float RectLength=1)
        {
            Texture2D texture2D = new Texture2D(1,1);
            texture2D.LoadImage(File.ReadAllBytes(KazimierInitializer.ModPath + "/ArtWork/"+path+".png"));
            Sprite sprite = obj.GetComponent<SpriteRenderer>().sprite;
            obj.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float)texture2D.width*ReactWidth, (float)texture2D.height* RectLength), new Vector2(0.5f, 0.5f), sprite.pixelsPerUnit, 0U, SpriteMeshType.FullRect);
        }
    }
}
