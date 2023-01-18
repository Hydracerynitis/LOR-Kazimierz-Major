using UnityEngine;
using System.Collections.Generic;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2160013 : PassiveAbilityBase
    {
        public override void OnRoundStartAfter()
        {
            base.OnRoundStart();
            owner.bufListDetail.AddBuf(new Barrier(owner.cardSlotDetail.PlayPoint));
        }
        public class Barrier : BattleUnitBuf
        {
            private int Light = 0;
            public Barrier(int light)
            {
                Light = light;
            }

            public override int GetDamageReductionRate()
            {
                return 3*Light;
            }
            public override int GetBreakDamageReductionRate()
            {
                return 3 * Light;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}
