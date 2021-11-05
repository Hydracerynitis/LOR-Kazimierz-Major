using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class PassiveAbility_2060014 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            BattleUnitBuf_Force.AddBuf(this.owner, 0);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (this.IsDefenseDice(behavior.Detail))
                BattleUnitBuf_Force.AddBuf(this.owner, 1);
        }
    }
}
