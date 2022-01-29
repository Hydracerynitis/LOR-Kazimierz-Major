using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardAbility_Corrosion1Atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Corrosion" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            BattleUnitBuf_Corrosion.AddBuf(target, 1);
        }
    }
}
