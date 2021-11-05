using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LOR_DiceSystem;
using System;

namespace KazimierzMajor
{
    public class PassiveAbility_2060053 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction))
            {
                alive.RecoverHP(4);
                if (alive.IsBreakLifeZero())
                    break;
                alive.breakDetail.RecoverBreak(4);
            }
        }
    }
}
