using System;
using BaseMod;

namespace KazimierzMajor
{
	public class DiceCardSelfAbility_fearness1 : DiceCardSelfAbilityBase
	{
        public override void OnUseCard()
        {
            if (card != PassiveAbility_2160031.nightmare)
                return;
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 2 });
        }
    }
}