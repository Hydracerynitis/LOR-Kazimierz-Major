using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_ChargeLightHeal : DiceCardSelfAbilityBase
    {
        private bool isCost0 = false;
        public override void OnStartBattle()
        {
            if (card.card.GetCost() <= 0)
                isCost0 = true;
        }
        public override void OnUseCard()
        {
            owner.allyCardDetail.DrawCards(2);
            if (!isCost0)
                return;
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction);
            if (aliveList.Count <= 0)
                return;
            aliveList.Sort((x, y) => (int)((double)x.hp - (double)y.hp));
            aliveList[0].RecoverHP(20);
            KazimierInitializer.UpdateInfo(aliveList[0]);
        }
    }
}
