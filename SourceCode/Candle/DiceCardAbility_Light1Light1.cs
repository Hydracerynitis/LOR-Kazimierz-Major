using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_Light1Light1 : DiceCardAbilityBase
    {
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            for(int i=0;i<2; i++)
            {
                if (owner.PlayPoint - owner.cardSlotDetail.ReservedPlayPoint >= 1)
                {
                    owner.cardSlotDetail.LosePlayPoint(1);
                    target.bufListDetail.AddBuf(new LoseLight());
                    LightIndicator.RefreshLight(owner);
                }
            }
        }
        public class LoseLight: BattleUnitBuf
        {
            public override void OnRoundStartAfter()
            {
                base.OnRoundStartAfter();
                if (_owner.PlayPoint > 0)
                    _owner.cardSlotDetail.LosePlayPoint(1);
                Destroy();
            }
        }
    }
}
