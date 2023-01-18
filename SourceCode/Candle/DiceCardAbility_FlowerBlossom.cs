using UnityEngine;
using System.Collections;
using HarmonyLib;
using System;
using EmotionalFix;
using LOR_DiceSystem;
using static UnityEngine.EventSystems.EventTrigger;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class DiceCardAbility_FlowerBlossom : DiceCardAbilityBase
    {
        private List<BattleUnitModel> targets = new List<BattleUnitModel>();
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            base.OnSucceedAreaAttack(target);
            if (targets.Exists(x => x == target))
                return;
            targets.Add(target);
            DiceBehaviour NewXml = behavior.behaviourInCard.Copy();
            NewXml.Min = 12;
            NewXml.Dice = 16;
            BattleVoidBehaviour.ExtraHit(target, behavior,NewXml, 0);
            BattleVoidBehaviour.ExtraHit(target, behavior, NewXml, 0);
            BattleVoidBehaviour.ExtraHit(target, behavior, NewXml, 0);
        }
    }
}
