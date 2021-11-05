using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_Crippling : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            this.owner.battleCardResultLog?.SetCreatureEffectSound("Creature/ButterFlyMan_Lock");
            this.card.target.bufListDetail.AddBuf(new Cripple());
        }
        public class Cripple : BattleUnitBuf
        {
            private int _count = 0;
            public override int SpeedDiceBreakedAdder() => 1;
            public override void OnRoundStart()
            {
                _count += 1;
                if (_count < 2)
                    return;
                this.Destroy();
            }
        }
    }
}
