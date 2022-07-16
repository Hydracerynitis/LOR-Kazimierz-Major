using System;

namespace KazimierzMajor
{
	public class DiceCardSelfAbility_kehan1_p : DiceCardSelfAbilityBase
	{
		public override void OnStartBattle()
		{
			foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
				unit.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.DmgUp, 3, base.owner);
		}
	}
}