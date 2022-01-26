using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_Light2Power2 : DiceCardSelfAbilityBase
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
            if (owner.PlayPoint - owner.cardSlotDetail.ReservedPlayPoint >= 2)
            {
                owner.cardSlotDetail.LosePlayPoint(2);
                card.ApplyDiceStatBonus(DiceMatch.AllAttackDice, new DiceStatBonus() { power=2});
                LightIndicator.RefreshLight(owner);
            }
        }
    }
}
