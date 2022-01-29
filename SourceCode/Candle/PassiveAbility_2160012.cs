using UnityEngine;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class PassiveAbility_2160012 : PassiveAbilityBase
    {
        private Queue<int> Priority = new Queue<int>();
        public override int SpeedDiceNumAdder()
        {
            return Singleton<StageController>.Instance.RoundTurn%4==0? 1 : 0;
        }
        public override void OnRoundStartAfter()
        {
            owner.allyCardDetail.ExhaustAllCards();
            Priority.Clear();
            for (int i = 100; i >= 0; i -= 10)
                Priority.Enqueue(i);
            List<int> cards = new List<int>();
            if (Singleton<StageController>.Instance.RoundTurn % 4 == 0)
                cards.Add(2160107);
            cards.AddRange(new List<int>() { 2160105, 2160103, 2160104, 2160106, 2160102, 2160101  });
            Harmony_Patch.AddNewCard(owner, cards, Priority);
        }
    }
}
