using System;

namespace KazimierzMajor
{
	public class DiceCardSelfAbility_kehan1 : DiceCardSelfAbilityBase
	{
		public override void OnStartBattle()
		{
			owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.DmgUp, 3, base.owner);
		}
	}
}