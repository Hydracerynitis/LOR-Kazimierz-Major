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
    public class FarAreaEffect_Effect01_Yaoyanghanshou : FarAreaEffect
    {
        private float _elapsed;
        private ActionDetail _beforeMotion;

        public override void Init(BattleUnitModel self, params object[] args)
        {
            base.Init(self, args);
            this.OnEffectStart();
            this._elapsed = 0.0f;
            new List<BattleUnitModel>() { self }.AddRange((IEnumerable<BattleUnitModel>)BattleObjectManager.instance.GetAliveList(self.faction == Faction.Enemy ? Faction.Player : Faction.Enemy));
            this._beforeMotion = ActionDetail.Default;
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(self.faction == Faction.Enemy ? Faction.Player : Faction.Enemy);
            /*foreach (BattleFarAreaPlayManager.VictimInfo victimInfo in DiceCardAbility_RadiantSum.subTarget)
            {
                if (victimInfo.unitModel != null)
                    aliveList.Remove(victimInfo.unitModel);
            }
            foreach (BattleUnitModel battleUnitModel in aliveList)
                SingletonBehavior<DiceEffectManager>.Instance.CreateBehaviourEffect("Effect01_Yaoyanghanshou", 1f, this._self.view, battleUnitModel.view);*/
            SingletonBehavior<DiceEffectManager>.Instance.CreateBehaviourEffect("Effect01_Yaoyanghanshou", 1f, this._self.view, BattleFarAreaPlayManager.Instance.attacker.currentDiceAction.target.view);
        }

        public override void Update()
        {
            if (this.state == FarAreaEffect.EffectState.Start)
            {
                if (!this._self.moveDetail.isArrived)
                    return;
                this._elapsed += Time.deltaTime;
                if ((double)this._elapsed < 3.3)
                    return;
                this._self.view.charAppearance.ChangeMotion(ActionDetail.S5);
                this.state = FarAreaEffect.EffectState.GiveDamage;
                this._elapsed = 0.0f;
            }
            else if (this.state == FarAreaEffect.EffectState.GiveDamage)
            {
                this._elapsed += Time.deltaTime;
                if ((double)this._elapsed < 0.3)
                    return;
                this._self.view.charAppearance.ChangeMotion(ActionDetail.S5);
                this._elapsed = 0.0f;
                this.isRunning = false;
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
                Destroy(this.gameObject);
            }
        }
    }
    public class BehaviourAction_Effect01_Yaoyanghanshou : BehaviourActionBase
    {
        public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
        {
            this._self = self;
            FarAreaEffect_Effect01_Yaoyanghanshou effect01Yaoyanghanshou = new GameObject().AddComponent<FarAreaEffect_Effect01_Yaoyanghanshou>();
            effect01Yaoyanghanshou.Init(self);
            return (FarAreaEffect)effect01Yaoyanghanshou;
        }
    }
    public class DiceAttackEffect_Effect01_Yaoyanghanshou : Battle.DiceAttackEffect.DiceAttackEffect
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
            AssetBundle assetBundle = KazimierInitializer.assetBundle["耀阳颔首"];
            this.Main = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("耀阳颔首1"));
            this.Main.layer = 8;
            this.Main.transform.parent = this._selfTransform;
            this.Main.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            if (atkdir == Direction.LEFT)
                Main.transform.localScale = new Vector3(-Main.transform.localScale.x, Main.transform.localScale.y, Main.transform.localScale.z);
            Transform[] componentsInChildren1 = this.Main.GetComponentsInChildren<Transform>(true);
            for (int index = 0; index < componentsInChildren1.Length; ++index)
            {
                if (componentsInChildren1[index].gameObject.GetComponent<Renderer>() != null)
                {
                    if (componentsInChildren1[index].gameObject.GetComponent<Renderer>().sortingOrder < 0)
                        componentsInChildren1[index].gameObject.layer = 20;
                    if (componentsInChildren1[index].gameObject.GetComponent<Renderer>().sortingOrder >= 0)
                        componentsInChildren1[index].gameObject.layer = 8;
                }
            }
            this.Second = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("耀阳颔首2"));
            this.Second.layer = 8;
            this.Second.transform.parent = this._targetTransform;
            this.Second.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            if (atkdir == Direction.LEFT)
                Second.transform.localScale = new Vector3(-Second.transform.localScale.x, Second.transform.localScale.y, Second.transform.localScale.z);
            Transform[] componentsInChildren2 = this.Second.GetComponentsInChildren<Transform>(true);
            for (int index = 0; index < componentsInChildren2.Length; ++index)
            {
                if (componentsInChildren2[index].gameObject.GetComponent<Renderer>() != null)
                {
                    if (componentsInChildren2[index].gameObject.GetComponent<Renderer>().sortingOrder < 0)
                        componentsInChildren2[index].gameObject.layer = 20;
                    if (componentsInChildren2[index].gameObject.GetComponent<Renderer>().sortingOrder >= 0)
                        componentsInChildren2[index].gameObject.layer = 8;
                }
            }
        }

        public override void Update()
        {
            this.time += Time.deltaTime;
            if (!(Main != null) || time < 30.0)
                return;
            Destroy(Main);
            Destroy(gameObject);
        }
    }
}
