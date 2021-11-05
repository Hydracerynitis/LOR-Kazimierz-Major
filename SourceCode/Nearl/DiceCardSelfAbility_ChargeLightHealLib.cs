using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_ChargeLightHealLib : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            owner.allyCardDetail.DrawCards(1);
            if(BattleUnitBuf_ChargeLight.GetBuff(this.owner,out BattleUnitBuf_ChargeLight buf) && buf.stack > 6)
            {
                buf.UseStack(2);
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction);
                if (aliveList.Count <= 0)
                    return;
                aliveList.Sort((x, y) => (int)((double)x.hp - (double)y.hp));
                aliveList[0].RecoverHP(5);
                Harmony_Patch.UpdateInfo(aliveList[0]);
            }
        }
    }
}
