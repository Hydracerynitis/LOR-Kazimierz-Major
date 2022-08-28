using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_Discard1Atk : DiceCardAbilityBase
    {
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.allyCardDetail.DiscardInHand(1);
        }
    }
}
