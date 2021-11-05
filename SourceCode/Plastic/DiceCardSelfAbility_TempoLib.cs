using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_TempoLib : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            BattleUnitBuf_Monmentum.AddBuf(this.owner, 2);
            BattleUnitBuf_Force.AddBuf(this.owner,2);
        }
    }
}
