using System;

namespace KazimierzMajor
{
	// Token: 0x0200004D RID: 77
	public class BattleUnitBuf_fearness : BattleUnitBuf
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000110 RID: 272
		protected override string keywordId
		{
			get
			{
				return "Fearness";
			}
		}

		protected override string keywordIconId
		{
			get
			{
				return "Fearness";
			}
		}

		// Token: 0x06000111 RID: 273
		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				max = -3,
				dmgRate = -50,
				breakRate = -50
			});
		}

		// Token: 0x06000112 RID: 274
		public override void OnRoundEnd()
		{
			this.Destroy();
		}

		// Token: 0x06000113 RID: 275
		public BattleUnitBuf_fearness()
		{
		}
	}
}
