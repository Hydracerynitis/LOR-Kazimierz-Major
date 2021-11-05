using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardAbility_Other1SameBreak : DiceCardAbilityBase
    {
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction).FindAll(x => x!=target);
            if (aliveList.Count > 0)
            {
                BattleUnitModel battleUnitModel = RandomUtil.SelectOne<BattleUnitModel>(aliveList);
                battleUnitModel.TakeBreakDamage(this.behavior.DiceResultDamage);
            }

        }
    }
}
