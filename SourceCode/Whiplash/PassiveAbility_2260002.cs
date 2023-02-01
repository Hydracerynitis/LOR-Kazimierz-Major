using UnityEngine;
using System.Collections;
using System;
using LOR_DiceSystem;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2260002 : PassiveAbilityBase
    {
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (StageController.Instance.GetAllCards().FindAll(x => x.owner == unit).Count >= 3)
                {
                    unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1);
                    unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 1);
                }
            }
            
        }
    }
}
