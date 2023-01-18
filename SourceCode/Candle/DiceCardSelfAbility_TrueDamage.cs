using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_TrueDamage: DiceCardSelfAbilityBase
    {
        public override bool IsTrueDamage()
        {
            return true;
        }
    }
}
