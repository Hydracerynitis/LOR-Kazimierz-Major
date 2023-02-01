using System;
using BaseMod;
using LOR_DiceSystem;

namespace KazimierzMajor
{
	public class DiceCardSelfAbility_SelfAllyDraw: DiceCardSelfAbilityBase
	{
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.allyCardDetail.DrawCards(1);
            RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList(owner.faction))?.allyCardDetail.DrawCards(1);
        }
    }
}