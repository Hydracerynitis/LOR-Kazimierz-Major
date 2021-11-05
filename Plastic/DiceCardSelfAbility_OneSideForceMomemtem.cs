using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_OneSideForceMomemtem : DiceCardSelfAbilityBase
    {
        public override void OnStartOneSideAction()
        {
            base.OnStartOneSideAction();
            BattleUnitBuf_Force.AddBuf(owner, 1);
            BattleUnitBuf_Monmentum.AddBuf(owner, 1);
        }
    }
}
