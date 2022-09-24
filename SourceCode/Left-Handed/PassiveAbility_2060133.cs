using System;
using System.Threading;

namespace KazimierzMajor
{
    public class PassiveAbility_2060133 : PassiveAbilityBase
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
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if (_activated && !hasActivated)
            {
                this.owner.RecoverHP((int)(this.owner.MaxHp*0.4));
                this.owner.breakDetail.RecoverBreakLife(this.owner.MaxBreakLife);
                this.owner.breakDetail.nextTurnBreak = false;
                this.owner.breakDetail.RecoverBreak(this.owner.breakDetail.GetDefaultBreakGauge());
                hasActivated = true;
            }
        }
    }
}
