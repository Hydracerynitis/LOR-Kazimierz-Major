using UnityEngine;
using System.Collections;
using System;
using LOR_DiceSystem;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2261003 : PassiveAbilityBase
    {
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (StageController.Instance.GetAllCards().FindAll(x => x.owner == unit).Count >= 3)
                {
                    int stack = 0;
                    if (unit.bufListDetail.HasBuf<BattleUnitBuf_Shield>())
                        stack = unit.bufListDetail.FindBuf<BattleUnitBuf_Shield>().stack;
                    int increase = Math.Min(stack + 3, 10) - stack;
                    unit.bufListDetail.AddBufByEtc<BattleUnitBuf_Shield>(increase);
                }
            }
            
        }
    }
}
