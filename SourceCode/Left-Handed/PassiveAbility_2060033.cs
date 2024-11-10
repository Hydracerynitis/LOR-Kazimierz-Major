using System;
using System.Threading;

namespace KazimierzMajor
{
    public class PassiveAbility_2060033 : PassiveAbilityBase
    {
        private bool hasActivated = false;
        private bool _activated = false;
        public override bool isImmortal => !hasActivated;
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (!this._activated && (double)this.owner.hp <= (double)dmg)
                this._activated = true;
            return false;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (hasActivated)
            {
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2);
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 2);
            }
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if(_activated && !hasActivated)
            {
                this.owner.RecoverHP(this.owner.MaxHp);
                this.owner.breakDetail.RecoverBreakLife(this.owner.MaxBreakLife);
                this.owner.breakDetail.nextTurnBreak = false;
                this.owner.breakDetail.RecoverBreak(this.owner.breakDetail.GetDefaultBreakGauge());
                this.owner.cardSlotDetail.RecoverPlayPoint(owner.cardSlotDetail.GetMaxPlayPoint());
                hasActivated = true;
            }
        }
    }
}
