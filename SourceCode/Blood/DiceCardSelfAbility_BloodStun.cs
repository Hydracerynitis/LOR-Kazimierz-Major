﻿using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_BloodStun : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "BloodStunDesc" };
        public override void OnUseCard()
        {
            BattleUnitModel target = card.target;
            BattleUnitBuf_BloodStun.AddBuf(target, owner);
            target.breakDetail.breakGauge = 0;
            target.breakDetail.breakLife = 0;
            target.breakDetail.DestroyBreakPoint();
            card.GetDiceBehaviorList().ForEach(x => x.SetBlocked(true));
            base.OnUseCard();
        }
    }
}
