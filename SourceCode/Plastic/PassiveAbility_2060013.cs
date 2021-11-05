using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class PassiveAbility_2060013 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            BattleUnitBuf_Monmentum.AddBuf(this.owner, 0);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (this.IsAttackDice(behavior.Detail))
                BattleUnitBuf_Monmentum.AddBuf(this.owner, 1);
        }
    }
}
