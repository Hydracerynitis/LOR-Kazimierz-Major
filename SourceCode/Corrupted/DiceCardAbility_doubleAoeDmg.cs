using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor 
{
    public class DiceCardAbility_doubleAoeDmg : DiceCardAbilityBase
    {
        public int splashdamage;
        public override void OnSucceedAttack()
        {
            splashdamage = this.behavior.DiceResultDamage;
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction).FindAll(x => x != behavior.card.target))
            {
                battleUnitModel.TakeDamage(splashdamage);
                owner.battleCardResultLog.SetSucceedAtkEvent(() => KazimierInitializer.UpdateInfo(battleUnitModel));
            }
        }
    }
}