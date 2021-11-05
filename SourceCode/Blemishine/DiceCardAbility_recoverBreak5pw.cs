using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardAbility_recoverBreak5pw : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            this.owner.breakDetail.RecoverBreak(5);
        }
    }
}
