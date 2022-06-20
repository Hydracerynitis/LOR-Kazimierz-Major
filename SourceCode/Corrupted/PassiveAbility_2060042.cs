using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class PassiveAbility_2060042 : PassiveAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction).FindAll(x => x != behavior.card.target))
            {
                battleUnitModel?.TakeDamage(4);
                owner.battleCardResultLog.SetSucceedAtkEvent(() => KazimierInitializer.UpdateInfo(battleUnitModel));
            }
        }
    }
}
