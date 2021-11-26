using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    public class PassiveAbility_2160124 :PassiveAbilityBase
    {
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = curCard.target.bufListDetail.GetActivatedBufList().FindAll(x => x is Target).Count });
            curCard.target.bufListDetail.AddBuf(new Target());
        }
        public class Target: BattleUnitBuf
        {
            public override void OnRoundEnd() => Destroy();
        }
    }
}
