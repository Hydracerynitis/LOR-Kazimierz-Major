﻿using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_Light3Prot5 : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            if (owner.PlayPoint - owner.cardSlotDetail.ReservedPlayPoint >= 3)
            {
                owner.cardSlotDetail.LosePlayPoint(3);
                owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Protection, 5, owner);
                owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.BreakProtection, 5, owner);
                LightIndicator.RefreshLight(owner);
            }
        }
    }
}
