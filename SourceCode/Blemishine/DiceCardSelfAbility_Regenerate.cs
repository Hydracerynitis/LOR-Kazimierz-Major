using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_Regenerate : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(this.owner.faction))
            {
                battleUnitModel.bufListDetail.AddBuf(new Regenerate());
            }
        }
        public class Regenerate : BattleUnitBuf
        {
            public override void OnRoundEnd()
            {
                this._owner.RecoverHP((int)(this._owner.MaxHp * 0.1));
                this.Destroy();
            }
        }
    }
}
