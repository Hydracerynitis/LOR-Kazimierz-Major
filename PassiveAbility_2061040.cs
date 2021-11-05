using UnityEngine;
using System.Collections;
using System;
using Sound;

namespace KazimierzMajor
{
    public class PassiveAbility_2061040 : PassiveAbilityBase
    {
        private Battle.CreatureEffect.CreatureEffect aura;
        private bool _dead = false;
        public override int SpeedDiceNumAdder() => _dead ? 4 : 0;
        public override void OnRoundStart()
        {
            if (_dead)
            {
                this.owner.allyCardDetail.DrawCards(8);
                foreach (BattleDiceCardModel card in owner.allyCardDetail.GetHand())
                    card.SetCostToZero();
                if (this.aura != null)
                    return;
                this.aura = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("6/RedHood_Emotion_Aura", 1f, this.owner.view, this.owner.view, -1f);
                SetParticle();
            }
        }
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if (_dead)
                return;
            if (unit.faction == this.owner.faction && unit.passiveDetail.HasPassive<PassiveAbility_2061040>())
                _dead = true;
        }
        public override void OnDie()
        {
            base.OnDie();
            this.DestroyAura();
        }
        private void DestroyAura()
        {
            if (this.aura != null && this.aura.gameObject != null)
                UnityEngine.Object.Destroy(this.aura.gameObject);
            this.aura = null;
        }
        private void SetParticle()
        {
            UnityEngine.Object original = Resources.Load("Prefabs/Battle/SpecialEffect/RedMistRelease_ActivateParticle");
            if (original != (UnityEngine.Object)null)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
                gameObject.transform.parent = this.owner.view.charAppearance.transform;
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.transform.localScale = Vector3.one;
            }
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Battle/Kali_Change");
        }
    }
}
