using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


namespace KazimierzMajor
{
    public class DiceCardSelfAbility_blood4 : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            BattleUnitBuf_Blood.AddBuf(owner, 4);
        }
    }
}
