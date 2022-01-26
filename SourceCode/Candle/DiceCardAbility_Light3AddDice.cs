using UnityEngine;
using System.Collections;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    public class DiceCardAbility_Light3AddDice: DiceCardAbilityBase
    {
        public override void OnLoseParrying()
        {
            if (owner.PlayPoint - owner.cardSlotDetail.ReservedPlayPoint >= 3)
            {
                owner.cardSlotDetail.LosePlayPoint(3);
                DiceBehaviour newDice = new DiceBehaviour() { Min = 9, Dice = 12, Type = BehaviourType.Atk, Detail = BehaviourDetail.Slash, MotionDetail = MotionDetail.F, EffectRes = "", Script = "", ActionScript = "", Desc = "" };
                BattleDiceBehavior dice = new BattleDiceBehavior() { behaviourInCard=newDice};
                card.AddDice(dice);
                LightIndicator.RefreshLight(owner);
            }
        }
    }
}
