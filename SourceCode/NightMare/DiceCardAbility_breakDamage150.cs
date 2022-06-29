using System;

namespace KazimierzMajor
{
	public class DiceCardAbility_breakDamage150 : DiceCardAbilityBase
	{
		public override void BeforeRollDice()
		{
			this.behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				breakRate = 50
			});
		}
	}
}