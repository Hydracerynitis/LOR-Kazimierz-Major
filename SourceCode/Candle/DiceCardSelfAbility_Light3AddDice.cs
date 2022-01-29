using UnityEngine;
using System.Collections;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_Light3AddDice: DiceCardSelfAbilityBase
    {
        public override void OnStartParrying()
        {
            for(int i=0; i<2; i++)
            {
                if (owner.PlayPoint - owner.cardSlotDetail.ReservedPlayPoint >= 3)
                {
                    owner.cardSlotDetail.LosePlayPoint(3);
                    DiceBehaviour newDice = new DiceBehaviour() { Min = 9, Dice = 12, Type = BehaviourType.Atk, Detail = BehaviourDetail.Slash, MotionDetail = MotionDetail.F, EffectRes = "", Script = "", ActionScript = "", Desc = "" };
                    BattleDiceBehavior dice = new BattleDiceBehavior() { behaviourInCard = newDice };
                    card.AddDice(dice);
                    LightIndicator.RefreshLight(owner);
                }
                else
                    return;
            }
        }
    }
}
