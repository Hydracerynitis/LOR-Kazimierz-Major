using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160127 :PassiveAbilityBase
    {
        public static List<BattleUnitModel> owners = new List<BattleUnitModel>();
        public override void OnWaveStart()
        {
            owners.Add(owner);
        }
        public override void OnDie()
        {
            owners.Remove(owner);
        }
    }
}
