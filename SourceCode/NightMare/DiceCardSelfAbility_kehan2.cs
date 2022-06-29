using System;

namespace KazimierzMajor
{
	public class DiceCardSelfAbility_kehan2 : DiceCardSelfAbilityBase
	{
		public override void OnStartBattle()
		{
			owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 2, base.owner);
			owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 2, base.owner);
		}
	}
}