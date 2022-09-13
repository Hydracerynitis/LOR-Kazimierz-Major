using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160028 : PassiveAbility_2160128
    {
        private Queue<int> Priority = new Queue<int>();
        public override void OnRoundStartAfter()
        {
            owner.allyCardDetail.ExhaustAllCards();
            Priority.Clear();
            for (int i = 100; i >= 0; i -= 10)
                Priority.Enqueue(i);
            KazimierInitializer.AddNewCard(owner, new List<int>() { 2160206, 2160206, 2160206 }, Priority);
        }
    }
}
