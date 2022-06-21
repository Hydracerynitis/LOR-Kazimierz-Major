using System;

namespace SN
{
	// Token: 0x0200000B RID: 11
	public class DiceCardAbility_targetPower_2 : DiceCardAbilityBase
	{
		// Token: 0x06000045 RID: 69 RVA: 0x0000234F File Offset: 0x0000054F
		public override void OnWinParrying()
		{
			base.behavior.TargetDice.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = -2 });
		}
	}
}