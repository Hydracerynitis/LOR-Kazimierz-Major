using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KazimierzMajor
{
    public class KhanEffectData
    {
        public static List<(BattleUnitModel, BattleUnitModel)> actor = new List<(BattleUnitModel, BattleUnitModel)>();
        public static List<BattleUnitModel> added = new List<BattleUnitModel>();
        public static Dictionary<BattleUnitModel, DiceCardXmlInfo> ownCard = new Dictionary<BattleUnitModel, DiceCardXmlInfo>();
        public static DiceBehaviour getIndex(DiceCardXmlInfo xml,int index)
        {
            int i = 0;
            int count = 1;
            List<DiceBehaviour> dices = xml.DiceBehaviourList.FindAll(x => x.Type == BehaviourType.Atk);
            while(count < index)
            {
                count++;
                i++;
                if (i >= dices.Count)
                    i = 0;
            }
            return dices[i];
        }
    }
    public class FarAreaEffect_Khan1: FarAreaEffect
    {
        private float _elapsed;
        private CameraFilterPack_FX_EarthQuake _camFilter;
        private SpriteRenderer _spr;
        private ActionDetail _beforeMotion;
        public override void Init(BattleUnitModel self, params object[] args)
        {
            base.Init(self, args);
            List<BattleUnitModel> enemies = new List<BattleUnitModel>(BattleObjectManager.instance.GetAliveList_opponent(self.faction));
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(self.faction))
                if(enemies.Find(x => x.index==unit.index || x.index>4) is BattleUnitModel target && !unit.breakDetail.IsBreakLifeZero())
                {
                    enemies.Remove(target);
                    KhanEffectData.actor.Add((unit,target));
                    if (StageController.Instance.AllyFormationDirection == Direction.RIGHT)
                    {
                        if (self.faction == Faction.Enemy)
                            unit.moveDetail.Move(new Vector3(target.view.WorldPosition.x - 5f, target.view.WorldPosition.y, target.view.WorldPosition.z), 200f);
                        else
                            unit.moveDetail.Move(new Vector3(target.view.WorldPosition.x + 5f, target.view.WorldPosition.y, target.view.WorldPosition.z), 200f);
                    }
                    else
                    {
                        if (self.faction != Faction.Enemy)
                            unit.moveDetail.Move(new Vector3(target.view.WorldPosition.x - 5f, target.view.WorldPosition.y, target.view.WorldPosition.z), 200f);
                        else
                            unit.moveDetail.Move(new Vector3(target.view.WorldPosition.x + 5f, target.view.WorldPosition.y, target.view.WorldPosition.z), 200f);
                    }
                    List<BattleDiceCardModel> cards = unit.allyCardDetail.GetAllDeck().FindAll(x => !KazimierInitializer.IsNotClashCard(x) && x.XmlData.DiceBehaviourList.Exists(y =>y.Type==BehaviourType.Atk));
                    if (cards.Count > 0)
                        KhanEffectData.ownCard.Add(unit, RandomUtil.SelectOne(cards).XmlData);
                }
            foreach (BattleUnitModel target in enemies)
            {
                BattleUnitModel copy = CopySelf(self);
                KhanEffectData.actor.Add((copy, target));
                KhanEffectData.added.Add(copy);
                if (StageController.Instance.AllyFormationDirection == Direction.RIGHT)
                {
                    if (self.faction == Faction.Enemy)
                        copy.moveDetail.Move(new Vector3(target.view.WorldPosition.x - 5f, target.view.WorldPosition.y, target.view.WorldPosition.z), 200f);
                    else
                        copy.moveDetail.Move(new Vector3(target.view.WorldPosition.x + 5f, target.view.WorldPosition.y, target.view.WorldPosition.z), 200f);
                }
                else
                {
                    if (self.faction != Faction.Enemy)
                        copy.moveDetail.Move(new Vector3(target.view.WorldPosition.x - 5f, target.view.WorldPosition.y, target.view.WorldPosition.z), 200f);
                    else
                        copy.moveDetail.Move(new Vector3(target.view.WorldPosition.x + 5f, target.view.WorldPosition.y, target.view.WorldPosition.z), 200f);
                }
            }
            this.OnEffectStart();
            this._elapsed = 0.0f;
            Singleton<BattleFarAreaPlayManager>.Instance.SetActionDelay(0.0f);
            this._beforeMotion = ActionDetail.Default;
        }
        public override void Update()
        {
            if (this.state == FarAreaEffect.EffectState.Start)
            {
                if (KhanEffectData.actor.Exists(x => !x.Item1.moveDetail.isArrived))
                    return;
                this.state = FarAreaEffect.EffectState.GiveDamage;
            }
            else if (this.state == FarAreaEffect.EffectState.GiveDamage)
            {
                this._elapsed += Time.deltaTime;
                if ((double)this._elapsed < 0.25)
                    return;
                this._beforeMotion = _self.view.charAppearance.GetCurrentMotionDetail();
                KhanEffectData.actor.ForEach(x => UpdateEffect_GiveDamage(x.Item1,x.Item2));
                this._elapsed = 0.0f;
                this.isRunning = false;
                this.state = FarAreaEffect.EffectState.End;
                this._camFilter = SingletonBehavior<BattleCamManager>.Instance?.EffectCam.gameObject.AddComponent<CameraFilterPack_FX_EarthQuake>();           
                if (SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject is ScorchedGirlMapManager)
                    this._spr = (SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject as ScorchedGirlMapManager).SetBurnFilterLinearDodge(true);
                TimeManager.Instance.SlowMotion(0.25f, 0.125f, true);
            }
            else if (this.state == FarAreaEffect.EffectState.End)
            {
                this._elapsed += Time.deltaTime;
                if (_camFilter != null)
                {
                    this._camFilter.Speed = (float)(30.0 * (1.0 - (double)this._elapsed));
                    this._camFilter.X = (float)(0.100000001490116 * (1.0 - (double)this._elapsed));
                    this._camFilter.Y = (float)(0.100000001490116 * (1.0 - (double)this._elapsed));
                }
                if (_spr != null)
                    this._spr.color = new Color(_spr.color.r,_spr.color.g,_spr.color.b,1f-_elapsed);
                if ((double)this._elapsed <= 1.0)
                    return;
                if (_camFilter != null)
                {
                    Destroy(_camFilter);
                    this._camFilter = (CameraFilterPack_FX_EarthQuake)null;
                }
                if (_spr != null)
                    this._spr.enabled = false;
                this.state = FarAreaEffect.EffectState.None;
                this._elapsed = 0.0f;
            }
            else
            {
                if (this.state != FarAreaEffect.EffectState.None)
                    return;
                if (this._self.Book.GetBookClassInfoId() == -1)
                    SingletonBehavior<BattleCamManager>.Instance.FollowUnits(false, BattleObjectManager.instance.GetAliveList());
                if (!this._self.view.FormationReturned)
                    return;
                Destroy(gameObject);
            }
        }
        private void UpdateEffect_GiveDamage(BattleUnitModel self, BattleUnitModel enemy)
        {
            if(KhanEffectData.ownCard.TryGetValue(self,out DiceCardXmlInfo value))
            {
                DiceBehaviour dice = KhanEffectData.getIndex(value, 1);
                self.view.charAppearance.ChangeMotion(MotionConverter.MotionToAction(dice.MotionDetail));
                self.view.charAppearance.soundInfo.PlaySound(dice.MotionDetail, true);
                SingletonBehavior<DiceEffectManager>.Instance.CreateBehaviourEffect(dice.EffectRes, 1f, self.view, enemy.view);
            }
            else
            {
                self.view.charAppearance.ChangeMotion(ActionDetail.Hit);
                self.view.charAppearance.soundInfo.PlaySound(MotionDetail.J, true);
                SingletonBehavior<DiceEffectManager>.Instance.CreateBehaviourEffect("Liu1_J", 1f, self.view, enemy.view);
            }  
        }
        public override void OnDisable()
        {
            base.OnDisable();
            if (_camFilter == null)
                return;
            Destroy(_camFilter);
            _camFilter = null;
        }
        private BattleUnitModel CopySelf(BattleUnitModel BattleData)
        {
            try
            {
                BattleUnitModel battleUnitModel = null;
                battleUnitModel = BattleObjectManager.CreateDefaultUnit(BattleData.faction);
                battleUnitModel._unitData = BattleData.UnitData;
                battleUnitModel.equipment.SetUnitData(BattleData.UnitData.unitData);
                battleUnitModel.formation = new FormationPosition(BattleData.formation._xmlInfo);
                battleUnitModel.OnCreated();
                KazimierInitializer.UpdateInfo(battleUnitModel);
                BattleObjectLayer.instance.AddUnit(battleUnitModel);
                return battleUnitModel;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            return null;
        }
    }
    public class FarAreaEffect_Khan2 : FarAreaEffect
    {
        private float _elapsed;
        private CameraFilterPack_FX_EarthQuake _camFilter;
        private SpriteRenderer _spr;
        private ActionDetail _beforeMotion;
        public override void Init(BattleUnitModel self, params object[] args)
        {
            base.Init(self, args);
            this.OnEffectStart();
            this._elapsed = 0.0f;
            Singleton<BattleFarAreaPlayManager>.Instance.SetActionDelay(0.0f);
            this._beforeMotion = ActionDetail.Default;
        }
        public override void Update()
        {
            if (this.state == FarAreaEffect.EffectState.Start)
            {
                if (KhanEffectData.actor.Exists(x => !x.Item1.moveDetail.isArrived))
                    return;
                this.state = FarAreaEffect.EffectState.GiveDamage;
            }
            else if (this.state == FarAreaEffect.EffectState.GiveDamage)
            {
                this._elapsed += Time.deltaTime;
                if ((double)this._elapsed < 0.25)
                    return;
                this._beforeMotion = _self.view.charAppearance.GetCurrentMotionDetail();
                KhanEffectData.actor.ForEach(x => UpdateEffect_GiveDamage(x.Item1, x.Item2));
                this._elapsed = 0.0f;
                this.isRunning = false;
                this.state = FarAreaEffect.EffectState.End;
                this._camFilter = SingletonBehavior<BattleCamManager>.Instance?.EffectCam.gameObject.AddComponent<CameraFilterPack_FX_EarthQuake>();
                if (SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject is ScorchedGirlMapManager)
                    this._spr = (SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject as ScorchedGirlMapManager).SetBurnFilterLinearDodge(true);
                TimeManager.Instance.SlowMotion(0.25f, 0.125f, true);
            }
            else if (this.state == FarAreaEffect.EffectState.End)
            {
                this._elapsed += Time.deltaTime;
                if (_camFilter != null)
                {
                    this._camFilter.Speed = (float)(30.0 * (1.0 - (double)this._elapsed));
                    this._camFilter.X = (float)(0.100000001490116 * (1.0 - (double)this._elapsed));
                    this._camFilter.Y = (float)(0.100000001490116 * (1.0 - (double)this._elapsed));
                }
                if (_spr != null)
                    this._spr.color = new Color(_spr.color.r, _spr.color.g, _spr.color.b, 1f - _elapsed);
                if ((double)this._elapsed <= 1.0)
                    return;
                if (_camFilter != null)
                {
                    Destroy(_camFilter);
                    this._camFilter = (CameraFilterPack_FX_EarthQuake)null;
                }
                if (_spr != null)
                    this._spr.enabled = false;
                this.state = FarAreaEffect.EffectState.None;
                this._elapsed = 0.0f;
            }
            else
            {
                if (this.state != FarAreaEffect.EffectState.None)
                    return;
                if (this._self.Book.GetBookClassInfoId() == -1)
                    SingletonBehavior<BattleCamManager>.Instance.FollowUnits(false, BattleObjectManager.instance.GetAliveList());
                if (!this._self.view.FormationReturned)
                    return;
                Destroy(gameObject);
            }
        }
        private void UpdateEffect_GiveDamage(BattleUnitModel self, BattleUnitModel enemy)
        {
            if (KhanEffectData.ownCard.TryGetValue(self, out DiceCardXmlInfo value))
            {
                DiceBehaviour dice = KhanEffectData.getIndex(value, 2);
                self.view.charAppearance.ChangeMotion(MotionConverter.MotionToAction(dice.MotionDetail));
                self.view.charAppearance.soundInfo.PlaySound(dice.MotionDetail, true);
                SingletonBehavior<DiceEffectManager>.Instance.CreateBehaviourEffect(dice.EffectRes, 1f, self.view, enemy.view);
            }
            else
            {
                self.view.charAppearance.ChangeMotion(ActionDetail.Slash);
                self.view.charAppearance.soundInfo.PlaySound(MotionDetail.H, true);
                SingletonBehavior<DiceEffectManager>.Instance.CreateBehaviourEffect("Liu1_H", 1f, self.view, enemy.view);
            } 
        }
        private void UpdateEffect_Ending(BattleUnitModel _self)
        {
            _self.view.charAppearance.ChangeMotion(this._beforeMotion);
        }
    }
    public class FarAreaEffect_Khan3 : FarAreaEffect
    {
        private float _elapsed;
        private CameraFilterPack_FX_EarthQuake _camFilter;
        private SpriteRenderer _spr;
        private ActionDetail _beforeMotion;
        public override void Init(BattleUnitModel self, params object[] args)
        {
            base.Init(self, args);
            this.OnEffectStart();
            this._elapsed = 0.0f;
            Singleton<BattleFarAreaPlayManager>.Instance.SetActionDelay(0.0f);
            this._beforeMotion = ActionDetail.Default;
        }
        public override void Update()
        {
            if (this.state == FarAreaEffect.EffectState.Start)
            {
                if (KhanEffectData.actor.Exists(x => !x.Item1.moveDetail.isArrived))
                    return;
                this.state = FarAreaEffect.EffectState.GiveDamage;
            }
            else if (this.state == FarAreaEffect.EffectState.GiveDamage)
            {
                this._elapsed += Time.deltaTime;
                if ((double)this._elapsed < 0.25)
                    return;
                this._beforeMotion = _self.view.charAppearance.GetCurrentMotionDetail();
                KhanEffectData.actor.ForEach(x => UpdateEffect_GiveDamage(x.Item1, x.Item2));
                this._elapsed = 0.0f;
                this.isRunning = false;
                this.state = FarAreaEffect.EffectState.End;
                this._camFilter = SingletonBehavior<BattleCamManager>.Instance?.EffectCam.gameObject.AddComponent<CameraFilterPack_FX_EarthQuake>();
                if (SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject is ScorchedGirlMapManager)
                    this._spr = (SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject as ScorchedGirlMapManager).SetBurnFilterLinearDodge(true);
                TimeManager.Instance.SlowMotion(0.25f, 0.125f, true);
            }
            else if (this.state == FarAreaEffect.EffectState.End)
            {
                this._elapsed += Time.deltaTime;
                if (_camFilter != null)
                {
                    this._camFilter.Speed = (float)(30.0 * (1.0 - (double)this._elapsed));
                    this._camFilter.X = (float)(0.100000001490116 * (1.0 - (double)this._elapsed));
                    this._camFilter.Y = (float)(0.100000001490116 * (1.0 - (double)this._elapsed));
                }
                if (_spr != null)
                    this._spr.color = new Color(_spr.color.r, _spr.color.g, _spr.color.b, 1f - _elapsed);
                if ((double)this._elapsed <= 1.0)
                    return;
                if (_camFilter != null)
                {
                    Destroy(_camFilter);
                    this._camFilter = (CameraFilterPack_FX_EarthQuake)null;
                }
                if (_spr != null)
                    this._spr.enabled = false;
                KhanEffectData.actor.ForEach(x => x.Item1.view.charAppearance.ChangeMotion(ActionDetail.Default));
                KhanEffectData.added.ForEach(x => x.view.StartDeadEffect());
                this.state = FarAreaEffect.EffectState.None;
                this._elapsed = 0.0f;
            }
            else
            {
                if (this.state != FarAreaEffect.EffectState.None)
                    return;
                if (this._self.Book.GetBookClassInfoId() == -1)
                    SingletonBehavior<BattleCamManager>.Instance.FollowUnits(false, BattleObjectManager.instance.GetAliveList());
                if (!this._self.view.FormationReturned)
                    return;
                Destroy(gameObject);
            }
        }
        private void UpdateEffect_GiveDamage(BattleUnitModel self, BattleUnitModel enemy)
        {
            if (KhanEffectData.ownCard.TryGetValue(self, out DiceCardXmlInfo value))
            {
                DiceBehaviour dice = KhanEffectData.getIndex(value, 3);
                self.view.charAppearance.ChangeMotion(MotionConverter.MotionToAction(dice.MotionDetail));
                self.view.charAppearance.soundInfo.PlaySound(dice.MotionDetail, true);
                SingletonBehavior<DiceEffectManager>.Instance.CreateBehaviourEffect(dice.EffectRes, 1f, self.view, enemy.view);
            }
            else
            {
                self.view.charAppearance.ChangeMotion(ActionDetail.Penetrate);
                self.view.charAppearance.soundInfo.PlaySound(MotionDetail.Z, true);
                SingletonBehavior<DiceEffectManager>.Instance.CreateBehaviourEffect("Liu1_Z", 1f, self.view, enemy.view);
            }        
        }
        public override void OnDisable()
        {
            KhanEffectData.added.ForEach(x => BattleObjectLayer.instance.RemoveUnit(x));
            KhanEffectData.ownCard.Clear();
            KhanEffectData.actor.Clear();
            KhanEffectData.added.Clear();
            base.OnDisable();
            if (_camFilter == null)
                return;
            Destroy(_camFilter);
            _camFilter = null;
        }
    }
    public class BehaviourAction_KhanAoe1: BehaviourActionBase
    {
        public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
        {
            _self = self;
            FarAreaEffect_Khan1 khan = new GameObject().AddComponent<FarAreaEffect_Khan1>();
            khan.Init(self);
            return khan;
        }
    }
    public class BehaviourAction_KhanAoe2 : BehaviourActionBase
    {
        public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
        {
            _self = self;
            FarAreaEffect_Khan2 khan = new GameObject().AddComponent<FarAreaEffect_Khan2>();
            khan.Init(self);
            return khan;
        }
    }
    public class BehaviourAction_KhanAoe3 : BehaviourActionBase
    {
        public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
        {
            _self = self;
            FarAreaEffect_Khan3 khan = new GameObject().AddComponent<FarAreaEffect_Khan3>();
            khan.Init(self);
            return khan;
        }
    }
}
