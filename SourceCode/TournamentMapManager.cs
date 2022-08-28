﻿using System;
using BaseMod;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sound;
using UnityEngine;
using System.IO;

namespace KazimierzMajor
{
    public class TournamentMapManager: CustomMapManager
    {
        public bool EasterEgg = false;
        private int loopIndex = 0;
        public bool HasKill = false;
        public bool Below50 = false;
        public override bool IsMapChangable()
        {
            return false;
        }
        private bool HasMachine() => /*false;*/ StageController.Instance.CurrentFloor == SephirahType.Keter;
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
            {
                if (HasMachine())
                {
                    name = "Order";
                    EasterEgg=true;
                }           
                else
                    name = "Amon";
            }
            AudioClip bgm = KazimierInitializer.BGM[name];
            mapBgm = new AudioClip[3] { bgm,bgm, bgm };
            mapSize = MapSize.L;
            _bMapInitialized = true;
            if (EasterEgg)
            {
                //Openning
                _creatureDlgIdList.Add("Ahhh......");
                _creatureDlgIdList.Add("free at last.");
                _creatureDlgIdList.Add("O Gabriel now dawn thy reckoning");
                _creatureDlgIdList.Add("And thy gore shall glisten before the temple of man.");
                _creatureDlgIdList.Add("Creature of steel.");
                _creatureDlgIdList.Add("My gratitude upon thee for my freedom");
                _creatureDlgIdList.Add("but the crime thy kind have commited against humanity are NOT forgotten.");
                _creatureDlgIdList.Add("And thy punishment");
                _creatureDlgIdList.Add("..................");
                _creatureDlgIdList.Add("is DEATH!");
                //Loop
                _creatureDlgIdList.Add("Judgement!");
                _creatureDlgIdList.Add("Die!");
                _creatureDlgIdList.Add("Crush!");
                _creatureDlgIdList.Add("Prepare thyself!");
                _creatureDlgIdList.Add("Thy end is now!");
                _dlgIdx = 0;
            }
        }
        public override void CreateDialog(Color txtColor)
        {
            if (HasKill)
            {
                PrintLine(txtColor, "Useless");
                HasKill = false;
                return;
            }
            else if (Below50)
            {
                PrintLine(txtColor, "Weak");
                Below50 = false;
                return;
            }
            if (_dlgIdx < 10)
                PrintLine(txtColor);
            else
            {
                if (_dlgIdx > 14)
                    _dlgIdx = 10;
                loopIndex = _dlgIdx;
                PrintLine(txtColor);
            }
            ++_dlgIdx;
        }
        public override void Update()
        {
            if (EasterEgg)
            {
                if (!CreatureDlgManagerUI.Instance.canvas.enabled)
                    CreatureDlgManagerUI.Instance.Init(BattleSceneRoot.Instance.currentMapObject = this);
                else
                {
                    if (_dlgEffect != null && _dlgEffect.gameObject != null)
                    {
                        if (!_dlgEffect.DisplayDone)
                            return;
                        CreateDialog(Color.cyan);
                    }
                    else
                        CreateDialog(Color.cyan);
                }
            } 
        }
        private void PrintLine(Color txtColor)
        {
            if (_dlgEffect != null && _dlgEffect.gameObject != null)
                _dlgEffect.FadeOut();
            _dlgEffect = SingletonBehavior<CreatureDlgManagerUI>.Instance.SetDlg(_creatureDlgIdList[_dlgIdx],txtColor);
        }
        private void PrintLine(Color txtColor,string msg)
        {
            if (_dlgEffect != null && _dlgEffect.gameObject != null)
                _dlgEffect.FadeOut();
            _dlgEffect = SingletonBehavior<CreatureDlgManagerUI>.Instance.SetDlg(msg, txtColor);
        }
        public override void EnableMap(bool b)
        {
            this.gameObject.SetActive(b); 
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            SingletonBehavior<BattleSoundManager>.Instance.SetEnemyTheme(mapBgm);
            if(EasterEgg && StageController.Instance.RoundTurn == 1)
            {
                BattleObjectManager.instance.GetAliveList(Faction.Enemy)[0].UnitData.unitData.SetTempName("Magerate Prime");
                BattleObjectManager.instance.InitUI();
            }
                

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
        public void ChangeToKhan()
        {
            GameObject bg = this.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
            DuplicateSprite(bg, "NigthMare");
            BattleSceneRoot.Instance._mapChangeFilter.StartMapChangingEffect(Direction.LEFT);
            SoundEffectManager.Instance.PlayClip("Creature / BossBird_Birth");
        }
    }
}
