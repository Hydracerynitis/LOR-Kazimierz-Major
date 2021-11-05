using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_BothUse1power1 : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            if(BattleUnitBuf_Monmentum.GetBuf(owner,out BattleUnitBuf_Monmentum monmentum) && monmentum.stack>=1 && BattleUnitBuf_Force.GetBuf(owner,out BattleUnitBuf_Force force) && force.stack >= 1)
            {
                monmentum.UseStack(1);
                force.UseStack(1);
                this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 1 });
            }
        }
    }
}
