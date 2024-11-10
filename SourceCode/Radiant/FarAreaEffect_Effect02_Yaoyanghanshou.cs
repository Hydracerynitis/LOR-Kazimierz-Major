using LOR_DiceSystem;
using Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KazimierzMajor
{
    public class FarAreaEffect_Effect02_Yaoyanghanshou : FarAreaEffect
    {
        private float _elapsed;
        private ActionDetail _beforeMotion;

        public override void Init(BattleUnitModel self, params object[] args)
        {
            base.Init(self, args);
            this.OnEffectStart();
            this._elapsed = 0.0f;
            this._beforeMotion = ActionDetail.Default;
        }

        public override void Update()
        {
            if (this.state == FarAreaEffect.EffectState.Start)
            {
                this._self.view.charAppearance.ChangeMotion(ActionDetail.S5);
                this.state = FarAreaEffect.EffectState.GiveDamage;
                this._elapsed = 0.0f;
            }
            else if (this.state == FarAreaEffect.EffectState.GiveDamage)
            {
                this._elapsed = 0.0f;
                this.state = FarAreaEffect.EffectState.End;
            }
            else if (this.state == FarAreaEffect.EffectState.End)
            {
                this.state = FarAreaEffect.EffectState.None;
                this._elapsed = 0.0f;
            }
            else
            {
                if (this.state != FarAreaEffect.EffectState.None || !this._self.view.FormationReturned)
                    return;
                Singleton<BattleFarAreaPlayManager>.Instance.SetActionDelay();
                Singleton<BattleFarAreaPlayManager>.Instance.SetRollDiceDelay();
                Singleton<BattleFarAreaPlayManager>.Instance.SetPrintRollDiceDelay();
                Singleton<BattleFarAreaPlayManager>.Instance.SetUIDelay();
                Destroy(gameObject);
            }
        }
    }
    public class BehaviourAction_Effect02_Yaoyanghanshou : BehaviourActionBase
    {
        public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
        {
            this._self = self;
            FarAreaEffect_Effect02_Yaoyanghanshou effect02Yaoyanghanshou = new GameObject().AddComponent<FarAreaEffect_Effect02_Yaoyanghanshou>();
            effect02Yaoyanghanshou.Init(self);
            return (FarAreaEffect)effect02Yaoyanghanshou;
        }

        public override float GetAreaAtkWaitActionDelay() => 0.0f;

        public override float GetAreaAtkRolldiceDelay() => 0.0f;
    }
    public class DiceAttackEffect_Effect02_Yaoyanghanshou : Battle.DiceAttackEffect.DiceAttackEffect
    {
        public Direction atkdir;
        public GameObject Main;
        private float time;
        public GameObject Second;

        public override void Initialize(BattleUnitView self, BattleUnitView target, float destroyTime)
        {
            this._bHasDamagedEffect = false;
            this._self = self.model;
            this._selfTransform = self.atkEffectRoot;
            this._targetTransform = target.atkEffectRoot;
            this.atkdir = (double)(target.WorldPosition - self.WorldPosition).x > 0.0 ? Direction.RIGHT : Direction.LEFT;
        }

        public override void Start()
        {
            this.Main = Instantiate(KazimierInitializer.assetBundle["耀阳颔首"].LoadAsset<GameObject>("耀阳颔首3"));
            this.Main.transform.parent = this._targetTransform;
            this.Main.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            this.Main.layer = 8;
            Transform[] componentsInChildren = this.Main.GetComponentsInChildren<Transform>(true);
            for (int index = 0; index < componentsInChildren.Length; ++index)
            {
                if (componentsInChildren[index].gameObject.GetComponent<Renderer>() != null)
                {
                    if (componentsInChildren[index].gameObject.GetComponent<Renderer>().sortingOrder < 0)
                        componentsInChildren[index].gameObject.layer = 22;
                    if (componentsInChildren[index].gameObject.GetComponent<Renderer>().sortingOrder >= 0)
                        componentsInChildren[index].gameObject.layer = 8;
                }
            }
            if (!this.atkdir.Equals((object)Direction.LEFT))
                return;
            this.Main.transform.Rotate(Vector3.up, 180f);
        }

        public override void Update()
        {
            this.time += Time.deltaTime;
            if (Main == null || time < 30.0)
                return;
            Destroy(Main);
            Destroy(gameObject);
        }
    }
}
