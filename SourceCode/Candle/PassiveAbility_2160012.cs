using UnityEngine;
using System.Collections.Generic;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2160012 : PassiveAbilityBase
    {
        private static readonly int[] CommonPage = new int[] { 2160101, 2160102, 2160103, 2160104, 2160105 };
        private static readonly int[] UncommonPage = new int[] { 2160106, 2160107, 2160108 };
        private static readonly int[] RarePage = new int[] { 2160109, 2160110, 2160111};
        private static readonly int[] UniquePage = new int[] { 2160112, 2160113, 2160114, 2160115 };
        private Queue<int> Priority = new Queue<int>();
        public override void OnRoundStartAfter()
        {
            owner.allyCardDetail.ExhaustAllCards();
            Priority.Clear();
            for (int i = 100; i >= 0; i -= 10)
                Priority.Enqueue(i);
            List<int> cards = new List<int>();
            CalculateCombination(cards, owner.cardSlotDetail.PlayPoint);
            for (int i = 0; i < 7; i++)
                cards.Add(RandomUtil.SelectOne(CommonPage));
            KazimierInitializer.AddNewCard(owner, cards, Priority);
        }
        public void CalculateCombination(List<int> cards, int currentLight)
        {
            BattleUnitBuf buf = owner.bufListDetail.FindBuf<InfiniteLight>();
            if (buf != null && buf.stack > 1)
            {
                cards.AddRange(UniquePage);
                return;
            }   
            else
            {
                if(currentLight >= 17)
                {
                    cards.Add(2160116);
                    cards.Add(RandomUtil.SelectOne(UniquePage));
                    cards.Add(RandomUtil.SelectOne(RarePage));
                    cards.Add(RandomUtil.SelectOne(RarePage));
                    currentLight -= 15;
                }
                if (currentLight >= 15)
                {
                    cards.Add(2160116);
                    cards.Add(RandomUtil.SelectOne(RarePage));
                    cards.Add(RandomUtil.SelectOne(RarePage));
                    currentLight -= 10;
                }
                if(currentLight>= 12)
                {
                    cards.Add(RandomUtil.SelectOne(UniquePage));
                    currentLight -= 5;
                }
                if (currentLight > 3)
                {
                    int count = currentLight / 2 - 1;
                    currentLight -= count * 2;
                    for (; count > 0; count--)
                        cards.Add(RandomUtil.SelectOne(RarePage));
                }
                for (; currentLight > 0; currentLight--)
                    cards.Add(RandomUtil.SelectOne(UncommonPage));
            }
        }
    }
}
