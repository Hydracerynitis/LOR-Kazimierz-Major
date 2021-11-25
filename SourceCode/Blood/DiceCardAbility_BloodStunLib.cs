using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_BloodStunLib : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "BloodStunDesc"};
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack();
            BattleUnitBuf_BloodStunLib.AddBuf(target, owner);
        }
    }
}
