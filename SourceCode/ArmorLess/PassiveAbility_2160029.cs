using UnityEngine;
using System.Collections;
using BaseMod;
using System;

namespace KazimierzMajor
{
    public class PassiveAbility_2160029 : PassiveAbility_2160000
    {
        public override void OnRoundStartAfter()
        {
            if (Singleton<StageController>.Instance.EnemyStageManager is EnemyTeamStageManager_ArmorLess armoless)
                desc = string.Format(Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(2160129)), armoless.Enemy.Count);
        }
    }
}

