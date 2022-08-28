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
    public class FarAreaEffect_Enough : FarAreaEffect
    {
        private bool HasClick = false;
        private float _elapsed;
        private CameraFilterPack_FX_EarthQuake _camFilter;
        private SpriteRenderer _spr;
        private ActionDetail _beforeMotion;
        private List<BattleUnitModel> enemies;
        public override void Init(BattleUnitModel self, params object[] args)
        {
            base.Init(self, args);
            if(args.Length > 0)
            {
                enemies  = (List<BattleUnitModel>)args[0];
                foreach (BattleUnitModel target in enemies)
                    target.moveDetail.Move(self, 25f);
            }
            this.OnEffectStart();
            this._elapsed = 0.0f;
            Singleton<BattleFarAreaPlayManager>.Instance.SetActionDelay(0.0f);
            this._beforeMotion = ActionDetail.Default;
        }
        public override void Update()
        {
            if (this.state == EffectState.Start)
            {
                this._elapsed += Time.deltaTime;
                if ((double)this._elapsed < 0.5f)
                    return;
                _elapsed = 0.0f;
                enemies.ForEach(x => x.moveDetail.Stop());
                state = EffectState.GiveDamage;
            }
            else if (this.state == FarAreaEffect.EffectState.GiveDamage)
            {
                if (!HasClick)
                {
                    SoundEffectPlayer.PlaySound("Creature/Greed_StrongAtk_Defensed");
                    _self.view.charAppearance.ChangeMotion(ActionDetail.Guard);
                    HasClick = true;
                }
                this._elapsed += Time.deltaTime;
                if ((double)this._elapsed < 0.5)
                    return;
                this._beforeMotion = _self.view.charAppearance.GetCurrentMotionDetail();
                this._elapsed = 0.0f;
                this.isRunning = false;
                _self.view.charAppearance.ChangeMotion(ActionDetail.Penetrate);
                _self.view.charAppearance.soundInfo.PlaySound(MotionDetail.Z, true);
                SingletonBehavior<DiceEffectManager>.Instance.CreateBehaviourEffect("Wedge_Z", 5f, _self.view, enemies[0].view).SetLayer("UI_WORLD");
                this.state = EffectState.End;
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
        public override void OnDisable()
        {
            base.OnDisable();
            if (_camFilter == null)
                return;
            Destroy(_camFilter);
            _camFilter = null;
        }
    }
    public class BehaviourAction_Enough : BehaviourActionBase
    {
        public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
        {
            _self = self;
            FarAreaEffect_Enough enough = new GameObject().AddComponent<FarAreaEffect_Enough>();
            List<BattleUnitModel> enemis = new List<BattleUnitModel>();
            BattleFarAreaPlayManager.Instance.victims.ForEach(x => enemis.Add(x.unitModel));
            enough.Init(self,enemis);
            return enough;
        }
    }
}
