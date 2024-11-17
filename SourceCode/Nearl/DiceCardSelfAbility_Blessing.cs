using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_Blessing : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            if (BattleUnitBuf_ChargeLight.GetBuff(owner, out BattleUnitBuf_ChargeLight buf) && buf.stack > 0)
            {
                BattleUnitBuf_Blessing.AddBuf(owner, owner, Math.Min(buf.stack, 3));
                buf.UseStack(buf.stack);
            }
            card.card.exhaust = true;
        }
    }
}
