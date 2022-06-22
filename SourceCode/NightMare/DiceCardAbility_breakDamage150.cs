using System;

namespace KazimierzMajor
{
	// Token: 0x02000A1F RID: 2591
	public class DiceCardAbility_breakDamage150 : DiceCardAbilityBase
	{
		// Token: 0x0600353C RID: 13628 RVA: 0x00132E93 File Offset: 0x00131093
		public override void BeforeRollDice()
		{
			this.behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				breakRate = 50
			});
		}
	}
}