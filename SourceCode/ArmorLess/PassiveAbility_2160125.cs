using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160125 :PassiveAbilityBase
    {
        List<BattleUnitModel> Annoyance = new List<BattleUnitModel>();
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            if (Annoyance.Contains(curCard.target))
            {
                curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 1 });
                Annoyance.Remove(curCard.target);
            }
        }
        public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel attackerCard)
        {
            if (!Annoyance.Contains(attackerCard.owner))
                Annoyance.Add(attackerCard.owner);
        }
    }
}
