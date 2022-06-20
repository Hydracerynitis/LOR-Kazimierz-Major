using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class PassiveAbility_2160000 : PassiveAbilityBase
    {
        public override int SpeedDiceNumAdder()
        {
            int num1 = 0;
            int num2 = this.owner.emotionDetail.SpeedDiceNumAdder();
            if (num2 > 0)
                num1 = -num2;
            return num1;
        }
        public override int MaxPlayPointAdder()
        {
            return -owner.emotionDetail.MaxPlayPointAdderByLevel();
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                unit.RecoverHP(unit.MaxHp);
                if (unit.emotionDetail.EmotionLevel < 5)
                {
                    unit.emotionDetail.CreateEmotionCoin(EmotionCoinType.Positive, 1);
                    unit.emotionDetail.CreateEmotionCoin(EmotionCoinType.Negative, 1);
                    unit.emotionDetail.SetEmotionLevel(5);
                    unit.cardSlotDetail.RecoverPlayPoint(unit.cardSlotDetail.GetMaxPlayPoint());
                    KazimierInitializer.UpdateInfo(unit);
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

