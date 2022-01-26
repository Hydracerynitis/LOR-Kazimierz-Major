using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160026 :PassiveAbilityBase
    {
        private Queue<int> Priority = new Queue<int>();
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (!IsAttackDice(behavior.Detail))
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { min = 4 * owner.cardSlotDetail.keepCard.GetDiceBehaviorList().Count, max = 4 * owner.cardSlotDetail.keepCard.GetDiceBehaviorList().Count });
        }
        public override void OnRoundStartAfter()
        {
            owner.allyCardDetail.ExhaustAllCards();
            Priority.Clear();
            for (int i = 100; i >= 0; i -= 10)
                Priority.Enqueue(i);
            Harmony_Patch.AddNewCard(owner, new List<int>() { 2160203, 2160203, 2160204}, Priority);
        }
    }
}
