using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class PassiveAbility_2060000 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            int level = Math.Max(Singleton<StageController>.Instance.GetCurrentStageFloorModel().team.emotionLevel, Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionLevel);
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
            {
                if (unit.emotionDetail.EmotionLevel < level)
                {
                    unit.emotionDetail.SetEmotionLevel(level);
                    unit.cardSlotDetail.RecoverPlayPoint(this.owner.cardSlotDetail.GetMaxPlayPoint());
                    Harmony_Patch.UpdateInfo(unit);
                }
            }
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            int emotionTotalCoinNumber = Singleton<StageController>.Instance.GetCurrentStageFloorModel().team.emotionTotalCoinNumber;
            Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionTotalBonus = emotionTotalCoinNumber + 1;
            Singleton<StageController>.Instance.GetStageModel().SetCurrentMapInfo(0);
        }
    }
}

