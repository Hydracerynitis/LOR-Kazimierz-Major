using UnityEngine;
using System.Collections;
using System;
using LOR_DiceSystem;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2260003 : PassiveAbilityBase
    {
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (StageController.Instance.GetAllCards().FindAll(x => x.owner == unit).Count >= 3)
                {
                    unit.bufListDetail.AddBufByEtc<BattleUnitBuf_Shield>(6);
                }
            }
            
        }
    }
}
