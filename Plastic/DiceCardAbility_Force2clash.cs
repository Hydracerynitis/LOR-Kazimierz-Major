using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class DiceCardAbility_Force2clash : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            BattleUnitBuf_Force.AddBuf(this.owner, 2);
        }
    }
}
