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
    public class DiceCardAbility_FlowerBlossomLib : DiceCardAbilityBase
    {
        private List<BattleUnitModel> targets = new List<BattleUnitModel>();
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            base.OnSucceedAreaAttack(target);
            if (targets.Exists(x => x == target))
                return;
            targets.Add(target);
            DiceBehaviour NewXml = behavior.behaviourInCard.Copy();
            NewXml.Min = 5;
            NewXml.Dice = 9;
            BattleVoidBehaviour.ExtraHit(target, behavior,NewXml, 0);
            BattleVoidBehaviour.ExtraHit(target, behavior, NewXml, 0);
            BattleVoidBehaviour.ExtraHit(target, behavior, NewXml, 0);
        }
    }
}
