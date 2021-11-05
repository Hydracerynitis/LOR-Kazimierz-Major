using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_Avatar : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            if (this.owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_2060051) is PassiveAbility_2060051 passive)
                this.owner.passiveDetail.PassiveList.Remove(passive);
            this.owner.passiveDetail.PassiveList.Add(new PassiveAbility_2060151(this.owner));
        }
    }
}
