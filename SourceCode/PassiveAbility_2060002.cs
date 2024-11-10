using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class PassiveAbility_2060002 : PassiveAbilityBase
    {
        public override void OnRoundEnd()
        {
            if (BattleObjectManager.instance.GetAliveList(this.owner.faction).Exists((Predicate<BattleUnitModel>)(x => x != this.owner)))
                return;
            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 2, this.owner);
            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, 2, this.owner);
        }
    }
}

