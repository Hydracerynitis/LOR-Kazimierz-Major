using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_InfiniteLight : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            owner.bufListDetail.AddBuf(new InfiniteLight());
        }        
        public class InfiniteLight: BattleUnitBuf
        {
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                owner.cardSlotDetail.RecoverPlayPoint(owner.cardSlotDetail.GetMaxPlayPoint());
            }
            public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
            {
                _owner.cardSlotDetail.RecoverPlayPoint(_owner.cardSlotDetail.GetMaxPlayPoint());
                LightIndicator.RefreshLight(_owner);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                _owner.cardSlotDetail.LosePlayPoint(_owner.cardSlotDetail.GetMaxPlayPoint());
                LightIndicator.RefreshLight(_owner);
                Destroy();
            }
        }
    }
}
