using BaseMod;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using HarmonyLib.Tools;
using LOR_BattleUnit_UI;
using LOR_DiceSystem;
using UnityEngine;
using static System.Reflection.Emit.OpCodes;
using static HarmonyLib.AccessTools;
using System.Linq;


namespace KazimierzMajor
{
    [HarmonyPatch]
    internal class ParryHP
    {
        [HarmonyPatch(typeof(BattleDiceBehavior), nameof(BattleDiceBehavior.GiveDamage))]
        [HarmonyPrefix]
        public static void BattleDiceBehavior_GiveDamage_Pre(BattleDiceBehavior __instance)
        {
            if (__instance._diceResultValue >= 10 && __instance.card.target.bufListDetail.HasBuf<PassiveAbility_2160153.CounterReady>())
            {
                __instance.SetBlocked(true);
                __instance.card.target.bufListDetail.RemoveBufAll(typeof(PassiveAbility_2160153.CounterReady));
                PassiveAbility_2160153.GetParried = __instance.owner.battleCardResultLog.CurbehaviourResult;
            }
        }
        [HarmonyPatch(typeof(BattleDiceCard_BehaviourDescUI), nameof(BattleDiceCard_BehaviourDescUI.SetBehaviourInfo))]
        [HarmonyPostfix]
        public static void BattleDiceCard_BehaviourDescUI_SetBehaviourInfo(BattleDiceCard_BehaviourDescUI __instance, DiceBehaviour behaviour, LorId cardId, List<DiceBehaviour> behaviourList)
        {
            if (PassiveAbility_2160053.ParryDict.TryGetValue(cardId, out List<ParryStruct> parry))
            {
                foreach (ParryStruct p in parry)
                {
                    if (behaviourList.IndexOf(behaviour) == p.index)
                    {
                        __instance.txt_ability.text = __instance.txt_ability.text.Insert(0, GetParryText(p.behaviour));
                        return;
                    }
                }
            }
        }
        public static string GetParryText(BehaviourDetail bd)
        {
            string output = "";
            switch (bd)
            {
                case BehaviourDetail.None:
                    output = BattleCardAbilityDescXmlList.Instance.GetAbilityDesc("ParryNone")[0];
                    break;
                case BehaviourDetail.Slash:
                    output = BattleCardAbilityDescXmlList.Instance.GetAbilityDesc("ParrySlash")[0];
                    break;
                case BehaviourDetail.Hit:
                    output = BattleCardAbilityDescXmlList.Instance.GetAbilityDesc("ParryHit")[0];
                    break;
                case BehaviourDetail.Penetrate:
                    output = BattleCardAbilityDescXmlList.Instance.GetAbilityDesc("ParryPenetrate")[0];
                    break;
            }
            return TextUtil.TransformConditionKeyword(output);
        }
        [HarmonyPatch(typeof(BattleDiceCardModel), nameof(BattleDiceCardModel.CreateDiceCardBehaviorList))]
        [HarmonyPostfix]
        public static void BattleDiceCardModel_CreateDiceCardBehaviorList(BattleDiceCardModel __instance, List<BattleDiceBehavior> __result)
        {
            if (__instance.owner != null && __instance.owner.passiveDetail.HasPassive<PassiveAbility_2160053>())
            {
                if (PassiveAbility_2160053.ParryDict.TryGetValue(__instance.GetID(), out List<ParryStruct> parry))
                    foreach (ParryStruct p in parry)
                        __result[p.index].AddAbility(new Parry(p.behaviour));
            }
        }

        [HarmonyPatch(typeof(RencounterManager), nameof(RencounterManager.SetMovingStateByActionResult))]
        [HarmonyPrefix]
        [HarmonyPriority(Priority.LowerThanNormal)]
        public static bool RencounterManager_SetMovingStateByActionResult(RencounterManager __instance, bool __runOriginal)
        {
            if (!__runOriginal)
                return false;
            RencounterManager.ActionAfterBehaviour actionAfterBehaviour = new RencounterManager.ActionAfterBehaviour() { infoList = new List<RencounterManager.MovingAction>(), result = Result.Win };
            RencounterManager.ActionAfterBehaviour loser = new RencounterManager.ActionAfterBehaviour() { infoList = new List<RencounterManager.MovingAction>(), result = Result.Lose };
            if (__instance._currentEnemyBehaviourResult == Parry.ParryTriggered || __instance._currentLibrarianBehaviourResult == PassiveAbility_2160153.GetParried && __instance._currentEnemyBehaviourResult.behaviourRawData != null)
            {
                __instance._currentEnemyBehaviourResult.diceBehaviourResult.result = Result.Win;
                __instance._currentLibrarianBehaviourResult.diceBehaviourResult.result = Result.Lose;
                DiceBehaviour xml = __instance._currentEnemyBehaviourResult.behaviourRawData.Copy();
                xml.Type = BehaviourType.Atk;
                __instance._currentEnemyBehaviourResult.behaviourRawData = xml;
                actionAfterBehaviour.view = __instance._enemy;
                loser.view = __instance._librarian;
                actionAfterBehaviour.data = __instance._currentEnemyBehaviourResult.diceBehaviourResult;
                loser.data = __instance._currentLibrarianBehaviourResult.diceBehaviourResult;
                actionAfterBehaviour.behaviourResultData = __instance._currentEnemyBehaviourResult;
                loser.behaviourResultData = __instance._currentLibrarianBehaviourResult;
                LoadParryBehaviourList(__instance, ref actionAfterBehaviour, ref loser);
                return false;
            }
            if (__instance._currentLibrarianBehaviourResult == Parry.ParryTriggered || __instance._currentEnemyBehaviourResult == PassiveAbility_2160153.GetParried && __instance._currentLibrarianBehaviourResult.behaviourRawData != null)
            {
                __instance._currentLibrarianBehaviourResult.diceBehaviourResult.result = Result.Win;
                __instance._currentEnemyBehaviourResult.diceBehaviourResult.result = Result.Lose;
                DiceBehaviour xml = __instance._currentLibrarianBehaviourResult.behaviourRawData.Copy();
                xml.Type = BehaviourType.Atk;
                __instance._currentLibrarianBehaviourResult.behaviourRawData = xml;
                actionAfterBehaviour.view = __instance._librarian;
                loser.view = __instance._enemy;
                actionAfterBehaviour.data = __instance._currentLibrarianBehaviourResult.diceBehaviourResult;
                loser.data = __instance._currentEnemyBehaviourResult.diceBehaviourResult;
                actionAfterBehaviour.behaviourResultData = __instance._currentLibrarianBehaviourResult;
                loser.behaviourResultData = __instance._currentEnemyBehaviourResult;
                LoadParryBehaviourList(__instance, ref actionAfterBehaviour, ref loser);
                return false;
            }
            return true;
        }
        public static void LoadParryBehaviourList(RencounterManager __instance, ref RencounterManager.ActionAfterBehaviour actionAfterBehaviour, ref RencounterManager.ActionAfterBehaviour loser)
        {
            if (loser.behaviourResultData.IsFarAtk())
                actionAfterBehaviour.infoList = new ParryFar().GetMovingAction(ref actionAfterBehaviour, ref loser);
            else
                actionAfterBehaviour.infoList = new ParryNear().GetMovingAction(ref actionAfterBehaviour, ref loser);
            actionAfterBehaviour.preventOverlap = true;
            loser.preventOverlap = true;
            __instance.StartCoroutine(__instance.MoveRoutine(actionAfterBehaviour, loser));
        }
    }
}
