using System;

namespace KazimierzMajor
{
	public class BattleUnitBuf_fearness : BattleUnitBuf
	{
		public bool Boost = false;
		public override string keywordId
		{
			get
			{
				return "Fearness";
			}
		}

		public override string keywordIconId
		{
			get
			{
				return "Fearness";
			}
		}
		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			int reduce = 50;
			if (Boost)
				reduce = 100;
			behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				max = -3,
				dmgRate = -reduce,
				breakRate = -reduce
			});
		}
		public override void OnRoundEnd()
		{
			Boost = false;
		}
		public BattleUnitBuf_fearness()
		{
			stack = 0;
		}
	}
}
