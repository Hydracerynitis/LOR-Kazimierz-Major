using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    public class PassiveAbility_2160021 :PassiveAbilityBase
    {
        private Queue<int> Priority = new Queue<int>();
        public override void OnDrawParrying(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = -75, breakRate = -75 });
            if (behavior.TargetDice?.Detail == BehaviourDetail.Guard)
                behavior.SetDamageRedution(behavior.TargetDice.DiceResultValue);
            behavior.GiveDamage(behavior.card.target);
        }
        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = -75, breakRate = -75 });
            if (behavior.TargetDice?.Detail == BehaviourDetail.Guard)
                behavior.SetDamageRedution(behavior.TargetDice.DiceResultValue);
            behavior.GiveDamage(behavior.card.target);
        }
        public override void OnRoundStartAfter()
        {
            owner.allyCardDetail.ExhaustAllCards();
            Priority.Clear();
            for (int i = 100; i >= 0; i -= 10)
                Priority.Enqueue(i);
            if(owner.Book.GetSpeedDiceRule(owner).speedDiceList.Count==4)
                Harmony_Patch.AddNewCard(owner, new List<int>() { 2160201, 2160201, 2160201, 2160202 }, Priority);
            else
                Harmony_Patch.AddNewCard(owner, new List<int>() { 2160201, 2160201, 2160202 }, Priority);
        }
    }
}
