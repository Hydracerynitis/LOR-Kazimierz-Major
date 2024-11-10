using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Reflection.Emit.OpCodes;
using static HarmonyLib.AccessTools;
using KI = KazimierzMajor.KazimierInitializer;

namespace KazimierzMajor
{
    [HarmonyPatch]
    static class LateAttackHP
    {
        static readonly Queue<BattlePlayingCardDataInUnitModel> LateCards = new Queue<BattlePlayingCardDataInUnitModel>();
        public static bool IsLateAttack(BattlePlayingCardDataInUnitModel card)
        {
            return card != null && card.cardAbility is DiceCardSelfAbility_LateAttack;
        }
        public static void OnChangeStagePhase(StageController.StagePhase _, StageController.StagePhase nextPhase)
        {
            if (nextPhase == StageController.StagePhase.WaitStartBattleEffect)
            {
                StageController.Instance.GetAllCards().RemoveAll(card =>
                {
                    if (!IsLateAttack(card))
                        return false;
                    LateCards.Enqueue(card);
                    return true;
                });
            }
            else if (nextPhase == StageController.StagePhase.SetCurrentDiceAction)
            {
                StageController controller = StageController.Instance;
                controller.ApplyAddedCardList();
                controller.RemoveUnusedCards();
                if (controller.GetAllCards().Count == 0)
                {
                    if (LateCards.Count > 0)
                    {
                        BattlePlayingCardDataInUnitModel nextCard = LateCards.Dequeue();
                        controller.AddAllCardListInBattle(nextCard, nextCard.target, nextCard.targetSlotOrder);
                        controller.ApplyAddedCardList();
                    }
                }
            }
            else if (nextPhase == StageController.StagePhase.RoundStartPhase_UI)
            {
                LateCards.Clear();
            }
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.StartParrying))]
        [HarmonyPrefix]
        [HarmonyPriority(Priority.LowerThanNormal)]
        static bool StageController_StartParrying_Pre(StageController __instance, BattlePlayingCardDataInUnitModel cardA
                    , BattlePlayingCardDataInUnitModel cardB, bool __runOriginal)
        {
            if (!__runOriginal)
            {
                return false;
            }
            if (!(IsLateAttack(cardA) || IsLateAttack(cardB)) || cardB.isKeepedCard)
            {
                return true;
            }
            __instance.phase = StageController.StagePhase.ExecuteOneSideAction;
            cardA.owner.turnState = BattleUnitTurnState.DOING_ACTION;
            cardA.target.turnState = BattleUnitTurnState.DOING_ACTION;
            BattleOneSidePlayManager.Instance.StartOneSidePlay(cardA);
            return false;
        }
    }
    [HarmonyPatch]
    static class FastAttackHP
    {
        public static List<BattlePlayingCardDataInUnitModel> FastCards = new List<BattlePlayingCardDataInUnitModel>();
        public static bool IsFastAttack(BattlePlayingCardDataInUnitModel card)
        {
            if (card == null)
                return false;
            if (FastCards.Contains(card))
                return true;
            if (card.cardAbility is DiceCardSelfAbility_OneSideFastAttack && KI.GetParry(card) == null)
                return true;
            if (card == PassiveAbility_2160031.nightmare)
                return true;
            return card.cardAbility is DiceCardSelfAbility_FastAttack;
        }
        [HarmonyPatch]
        static class CardSortPatch
        {
            [HarmonyTargetMethods]
            static IEnumerable<MethodBase> TargetMethods()
            {
                foreach (var nested in typeof(StageController).GetNestedTypes(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic))
                    foreach (var method in nested.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
                        if (method.ReturnType == typeof(int))
                        {
                            var param = method.GetParameters();
                            if (param.Length == 2)
                            {
                                if (param[0].ParameterType == typeof(BattlePlayingCardDataInUnitModel) && param[0].Name == "c1"
                                    && param[1].ParameterType == typeof(BattlePlayingCardDataInUnitModel) && param[1].Name == "c2")
                                    yield return method;
                            }
                        }
            }
            static void Postfix(BattlePlayingCardDataInUnitModel c1, BattlePlayingCardDataInUnitModel c2, ref int __result)
            {
                if (IsFastAttack(c1) && !IsFastAttack(c2))
                    __result = -1;
                else if (IsFastAttack(c2))
                    __result = 1;
            }
        }
        [HarmonyPatch]
        static class UnitSortPatch
        {
            [HarmonyTargetMethods]
            static IEnumerable<MethodBase> TargetMethods()
            {
                foreach (var nested in typeof(StageController).GetNestedTypes(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic))
                    foreach (var method in nested.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
                        if (method.ReturnType == typeof(int))
                        {
                            var param = method.GetParameters();
                            if (param.Length == 2)
                            {
                                if (param[0].ParameterType == typeof(BattleUnitModel) && param[0].Name == "u1"
                                    && param[1].ParameterType == typeof(BattleUnitModel) && param[1].Name == "u2")
                                    yield return method;
                            }
                        }
            }
            static void Postfix(BattleUnitModel u1, BattleUnitModel u2, ref int __result)
            {
                if (IsFastAttack(u1.currentDiceAction) && !IsFastAttack(u2.currentDiceAction))
                    __result = -1;
                else if (IsFastAttack(u2.currentDiceAction))
                    __result = 1;
            }
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.WaitUnitArrivePhase))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> StageController_WaitUnitArrivePhase_In(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            int index = codes.FindIndex(c => c.opcode == Stfld && (c.operand as FieldInfo)?.Name == "arrivedUnit");
            if (index >= 0)
            {
                codes.InsertRange(index + 1, new CodeInstruction[]
                {
                    new CodeInstruction(Ldloc_1),
                    new CodeInstruction(Ldloc_3),
                    new CodeInstruction(Call, Method(typeof(FastAttackHP), nameof(StartActionHelper)))
                });
            }
            return codes;
        }

        static void StartActionHelper(List<BattleUnitModel> activeUnits, List<BattleUnitModel> maxSpeedUnits)
        {
            foreach (var unit in activeUnits)
                if (IsFastAttack(unit.currentDiceAction))
                    maxSpeedUnits.Add(unit);
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.MoveUnitPhase))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> StageController_MoveUnitPhase_In(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            int index = codes.FindIndex(c => c.opcode == Stloc_3);
            if (index >= 0)
            {
                codes.InsertRange(index + 1, new CodeInstruction[]
                {
                    new CodeInstruction(Ldloc_0),
                    new CodeInstruction(Ldloc_3),
                    new CodeInstruction(Call, Method(typeof(FastAttackHP), nameof(StopUnitHelper)))
                });
            }
            return codes;
        }
        static void StopUnitHelper(List<BattleUnitModel> activeUnits, List<BattleUnitModel> stoppedUnits)
        {
            foreach (var unit in activeUnits)
            {
                if (IsFastAttack(unit.currentDiceAction))
                {
                    stoppedUnits.Add(unit);
                    if (unit.currentDiceAction.target?.currentDiceAction != null && !IsFastAttack(unit.currentDiceAction.target.currentDiceAction))
                        stoppedUnits.Add(unit.currentDiceAction.target);
                }
            }
        }
    }
}
