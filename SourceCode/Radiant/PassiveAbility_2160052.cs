using UnityEngine;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class PassiveAbility_2160052 : PassiveAbilityBase
    {
        private int SpecialCoolDown = 3;
        private int EnoughCoolDown = 2;
        public static readonly int[] StrongCard = new int[] { 2160504, 2160505, 2160506, 2160507, 2160508, 2160509, 2160510 };
        public static readonly int[] WeakCard = new int[] { 2160503, 2160511, 2160512, 2160513, 2160514, 2160515, 2160516 };
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                unit.SetKnockoutInsteadOfDeath(true);
            owner.allyCardDetail.ExhaustAllCards();
            Queue<int> priority = new Queue<int>();
            for (int i = 100; i > 0; i -= 10)
                priority.Enqueue(i);
            List<int> card = new List<int>() { };
            int aoeCount = 0;
            if (--SpecialCoolDown <= 0)
            {
                aoeCount++;
                card.Add(2160501);
                SpecialCoolDown = 3;
            }
            if (--EnoughCoolDown <= 0)
            {
                aoeCount++;
                card.Add(2160502);
                EnoughCoolDown = 2;
            }
            List<int> strongCandicate = new List<int>(StrongCard);
            for (int i = 0; i < (aoeCount>0? 4:5); i++)
            {
                int index = Random.Range(0, strongCandicate.Count);
                card.Add(strongCandicate[index]);
                strongCandicate.RemoveAt(index);
            }
            List<int> weakCandidate = new List<int>(WeakCard);
            for (int i = 0; i < (aoeCount>1?3:4); i++)
            {
                int index = Random.Range(0, weakCandidate.Count);
                card.Add(weakCandidate[index]);
                weakCandidate.RemoveAt(index);
            }
            KazimierInitializer.AddNewCard(owner, card, priority);
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            curCard.ignorePower = true;
        }
    }
}
