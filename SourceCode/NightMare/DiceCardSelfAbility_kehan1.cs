using System;

namespace KazimierzMajor
{
	public class DiceCardSelfAbility_kehan1 : DiceCardSelfAbilityBase
	{
		public override void OnStartBattle()
		{
			owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 3, base.owner);
			owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 3, base.owner);
		}
	}
}