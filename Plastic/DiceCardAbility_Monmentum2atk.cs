using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class DiceCardAbility_Monmentum2atk : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            BattleUnitBuf_Monmentum.AddBuf(this.owner, 2);
        }
    }
}
