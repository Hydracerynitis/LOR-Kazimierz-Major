using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class PassiveAbility_2060051 : PassiveAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            List<BattleUnitModel> friend = BattleObjectManager.instance.GetAliveList(this.owner.faction);
            BattleUnitModel battleUnitModel = RandomUtil.SelectOne<BattleUnitModel>(friend);
            battleUnitModel.RecoverHP(4);
            owner.battleCardResultLog.SetSucceedAtkEvent(() => KazimierInitializer.UpdateInfo(battleUnitModel));
        }
    }
}
