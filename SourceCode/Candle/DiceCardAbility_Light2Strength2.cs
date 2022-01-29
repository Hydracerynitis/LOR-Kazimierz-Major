using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_Light2Strength2 : DiceCardAbilityBase
    {
        public override void OnLoseParrying()
        {
            for(int i=0; i<2; i++)
            {
                if (owner.PlayPoint - owner.cardSlotDetail.ReservedPlayPoint >= 2)
                {
                    owner.cardSlotDetail.LosePlayPoint(2);
                    owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 2, owner);
                    LightIndicator.RefreshLight(owner);
                }
                else
                    return;
            }
        }
    }
}
