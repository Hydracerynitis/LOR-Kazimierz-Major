using UnityEngine;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class PassiveAbility_2160151 : PassiveAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            owner.breakDetail.RecoverBreak(6); //15
        }
        public override bool DontChangeResistByBreak()
        {
            return true;
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            curCard.ignorePower = true;
        }
    }
}
