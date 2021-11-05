using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class PassiveAbility_2060043 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            if (this.owner.IsBreakLifeZero())
                return;
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction))
            {
                if(!battleUnitModel.IsBreakLifeZero())
                    battleUnitModel.TakeBreakDamage(4);
            }
        }
    }
}
