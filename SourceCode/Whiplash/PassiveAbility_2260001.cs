using UnityEngine;
using System.Collections;
using System;
using LOR_DiceSystem;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2260001 : PassiveAbilityBase
    {
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (unit == owner)
                    continue;
                if (unit.cardSlotDetail.cardAry.Exists(x => x != null && owner.cardSlotDetail.cardAry.Exists(y => y!=null && x.target == y.target)))
                {
                    unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.DmgUp, 3);
                }
            }
            
        }
    }
}
