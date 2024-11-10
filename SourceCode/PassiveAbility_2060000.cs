using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class PassiveAbility_2060000 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            List<BattleUnitModel> enemy = BattleObjectManager.instance.GetAliveList(Faction.Enemy);
            int enemylevel = enemy.Sum(x => x.emotionDetail.EmotionLevel)/ enemy.Count;
            List<BattleUnitModel> ally = BattleObjectManager.instance.GetAliveList(Faction.Player);
            int allyLevel= ally.Sum(x => x.emotionDetail.EmotionLevel) / ally.Count;
            int level = Math.Max(enemylevel,allyLevel);
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
            {
                unit.RecoverHP(unit.MaxHp);
                if (unit.emotionDetail.EmotionLevel < level)
                {
                    unit.emotionDetail.CreateEmotionCoin(EmotionCoinType.Positive, 1);
                    unit.emotionDetail.CreateEmotionCoin(EmotionCoinType.Negative, 1);
                    unit.allyCardDetail.DrawCards(level - unit.emotionDetail.EmotionLevel);
                    unit.emotionDetail.SetEmotionLevel(level);
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
    public class PassiveAbility_2060001 : PassiveAbilityBase
    {

    }
}

