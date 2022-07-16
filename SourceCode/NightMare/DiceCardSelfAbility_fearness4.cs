using System;
using BaseMod;

namespace KazimierzMajor
{
	public class DiceCardSelfAbility_fearness4 : DiceCardSelfAbilityBase
	{
		public override void OnStartBattle()
		{
			card.target.bufListDetail.AddBuf(new ReduceDamage());
			card.subTargets.ForEach(x => x.target.bufListDetail.AddBuf(new ReduceDamage()));
		}
		public class ReduceDamage: BattleUnitBuf
        {
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
				behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 25, breakRate = 25 });
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
				Destroy();
            }
        }
	}
}