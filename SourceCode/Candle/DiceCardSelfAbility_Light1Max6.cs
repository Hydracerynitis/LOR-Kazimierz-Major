using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_Light1Max6: DiceCardSelfAbilityBase
    {
        public override void OnStartParrying()
        {
            base.OnStartParrying();
            OnUseCardAfter();
        }
        public override void OnStartOneSideAction()
        {
            base.OnStartOneSideAction();
            OnUseCardAfter();
        }
        public void OnUseCardAfter()
        {
            if (owner.PlayPoint - owner.cardSlotDetail.ReservedPlayPoint >= 1)
            {
                owner.cardSlotDetail.LosePlayPoint(1);
                card.ApplyDiceStatBonus(DiceMatch.AllAttackDice,new DiceStatBonus() { max = 6 });
                LightIndicator.RefreshLight(owner);
            }
        }
    }
}
