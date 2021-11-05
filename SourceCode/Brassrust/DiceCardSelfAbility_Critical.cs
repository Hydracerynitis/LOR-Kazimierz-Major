using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor_Mod.Brassrust
{
    public class DiceCardSelfAbility_Critical:DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return owner.hp<owner.MaxHp/4;
        }
    }
}
