using System;
using BaseMod;
using LOR_DiceSystem;

namespace KazimierzMajor
{
	public class DiceCardSelfAbility_SelfAllyEnergy : DiceCardSelfAbilityBase
	{
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.cardSlotDetail.RecoverPlayPoint(2);
            RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList(owner.faction))?.cardSlotDetail.RecoverPlayPoint(1);
        }
    }
}