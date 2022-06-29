using System;

namespace KazimierzMajor
{
	public class DiceCardAbility_targetPower_2 : DiceCardAbilityBase
	{
		public override void OnWinParrying()
		{
			base.behavior.TargetDice.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = -2 });
		}
	}
}