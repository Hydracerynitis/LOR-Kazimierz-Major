using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_BountyPublish : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            PassiveAbility_2160023.Bounty buf = card.target.bufListDetail.GetActivatedBufList().Find(x => x is PassiveAbility_2160023.Bounty) as PassiveAbility_2160023.Bounty;
            if (buf == null)
            {
                buf = new PassiveAbility_2160023.Bounty();
                card.target.bufListDetail.AddBuf(buf);
            }
            buf.Add(1);
        }
    }
}
