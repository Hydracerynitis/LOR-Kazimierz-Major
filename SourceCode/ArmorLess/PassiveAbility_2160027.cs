using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160027 :PassiveAbilityBase
    {
        private Queue<int> Priority = new Queue<int>();
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (behavior.IsParrying())
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { breakRate = 50 });
        }
        public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
        {
            int breakgauge = 9999;
            List<BattleUnitModel> lowest = new List<BattleUnitModel>();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                if (unit.IsBreakLifeZero())
                    continue;
                if (unit.breakDetail.breakGauge < breakgauge)
                {
                    lowest.Clear();
                    lowest.Add(unit);
                    breakgauge = unit.breakDetail.breakGauge;
                }
                else if (unit.breakDetail.breakGauge == breakgauge)
                    lowest.Add(unit);
            }
            if (lowest.Count <= 0)
                return base.ChangeAttackTarget(card, idx);
            else
                return RandomUtil.SelectOne<BattleUnitModel>(lowest);
        }
        public override void OnRoundStartAfter()
        {
            owner.allyCardDetail.ExhaustAllCards();
            Priority.Clear();
            for (int i = 100; i >= 0; i -= 10)
                Priority.Enqueue(i);
            KazimierInitializer.AddNewCard(owner, new List<int>() { 2160205, 2160205, 2160205 }, Priority);
        }
    }
}
