using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160041 :PassiveAbilityBase
    {
        private int accumulated;
        public override void OnLoseHp(int dmg)
        {
            base.OnLoseHp(dmg);
            accumulated += dmg;
            int stack = accumulated / 10;
            accumulated -= 10 * stack;
            BattleUnitBuf_Blood.AddBuf(owner, stack);
        }
    }
}
