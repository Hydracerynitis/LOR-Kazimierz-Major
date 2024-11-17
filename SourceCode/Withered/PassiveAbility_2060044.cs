using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class PassiveAbility_2060044 : PassiveAbilityBase
    {
        
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            int dmg = behavior.DiceResultValue / 2;
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction).FindAll(x => x!=behavior.card.target && !x.IsBreakLifeZero());
            for (int index = 2; aliveList.Count > 0 && index > 0; --index)
            {
                BattleUnitModel battleUnitModel = RandomUtil.SelectOne<BattleUnitModel>(aliveList);
                aliveList.Remove(battleUnitModel);
                battleUnitModel.TakeBreakDamage(dmg);
            }
        }
    }
}
