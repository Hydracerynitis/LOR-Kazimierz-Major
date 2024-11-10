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
    class RadiantAoeHP
    {
        [HarmonyPatch(typeof(BattleCardAbilityDescXmlList), nameof(BattleCardAbilityDescXmlList.GetDefaultAbilityDescString))]
        [HarmonyPostfix]
        public static void BattleCardAbilityDescXmlList_GetDefaultAbilityDescString(DiceCardXmlInfo card, ref string __result)
        {
            if (card.id == Tools.MakeLorId(2160501))
                __result = string.Empty;
        }
        [HarmonyPatch(typeof(BattleFarAreaPlayManager), nameof(BattleFarAreaPlayManager.RollVictimsDice))]
        [HarmonyPostfix]
        public static void BattleFarAreaPlayManager_RollVictimsDice(BattleFarAreaPlayManager.VictimInfo v)
        {
            BattlePlayingCardDataInUnitModel aoe = BattleFarAreaPlayManager.Instance.attacker.currentDiceAction;
            if (aoe.card.GetID() == Tools.MakeLorId(2160501) && v.unitModel == aoe.target && v.playingCard != null)
            {
                int sum = 0;
                foreach (BattleDiceBehavior dice in v.playingCard.GetDiceBehaviorList())
                {
                    if (dice != v.playingCard.currentBehavior)
                    {
                        dice.BeforeRollDice(null);
                        dice.RollDice();
                        dice.UpdateDiceFinalValue();
                        sum += dice.DiceResultValue;
                    }
                }
                if (v.playingCard.currentBehavior != null)
                    v.playingCard.currentBehavior._diceFinalResultValue += sum;
            }
        }
        [HarmonyPatch(typeof(BattleUnitTargetArrowManagerUI), nameof(BattleUnitTargetArrowManagerUI.UpdateTargetListData))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> BattleUnitTargetArrowManagerUI_UpdateTargetListData_Tran(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            int index = codes.FindIndex(c => c.opcode == Stfld && (c.operand as FieldInfo)?.Name == "data");
            if (index >= 0)
            {
                var data_field = codes[index].operand as FieldInfo;
                codes.InsertRange(index + 1, new CodeInstruction[]
                {
                    new CodeInstruction(Ldloc_S,(byte)6),
                    new CodeInstruction(Ldloc_S,(byte)8),
                    new CodeInstruction(Ldfld,data_field),
                    new CodeInstruction(Call, Method(typeof(RadiantAoeHP), nameof(AddPairing)))
                });
            }
            return codes;
        }
        static void AddPairing(BattlePlayingCardDataInUnitModel cardDataInUnitModel, BattleUnitTargetArrowData data)
        {
            if (cardDataInUnitModel.card.GetID() == Tools.MakeLorId(2160501))
                data.isPairing = true;
        }
    }
}
