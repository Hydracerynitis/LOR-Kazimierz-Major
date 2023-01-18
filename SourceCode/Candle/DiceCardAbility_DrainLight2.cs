using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_DrainLight2 : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            behavior.card.target.cardSlotDetail.LosePlayPoint(2);
            owner.cardSlotDetail.RecoverPlayPoint(2);
        }
    }
}
