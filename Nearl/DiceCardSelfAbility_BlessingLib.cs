using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_BlessingLib : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(this.owner.faction))
            {
                BattleUnitBuf_Blessing.AddBuf(battleUnitModel, this.owner, 1);
            }
            this.card.card.exhaust = true;
        }
    }
}
