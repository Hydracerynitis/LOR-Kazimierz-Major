using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMod;
using HarmonyLib;
using LOR_BattleUnit_UI;
using LOR_DiceSystem;
using UnityEngine;

namespace KazimierzMajor
{
    [HarmonyPatch]
    class MagerateHp
    {
        [HarmonyPatch(typeof(BattleDiceBehavior),nameof(BattleDiceBehavior.GiveDamage))]
        [HarmonyPostfix]
        public static void BattleDiceBehavior_GiveDamage(BattleDiceBehavior __instance)
        {
            PassiveAbility_2160055.Active = false;
        }
        [HarmonyPatch(typeof(BattleUnitModel),nameof(BattleUnitModel.GetDamageRate))]
        [HarmonyPostfix]
        public static void BattleUnitModel_GetDamageRate(ref float __result)
        {
            if(PassiveAbility_2160055.Active)
                __result = 1f;
        }
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.GetBreakDamageRate))]
        [HarmonyPostfix]
        public static void BattleUnitModel_GetBreakDamageRate(ref float __result)
        {
            if(PassiveAbility_2160055.Active)
                __result = 1f;
        }
        [HarmonyPatch(typeof(BattleDiceCard_BehaviourDescUI),nameof(BattleDiceCard_BehaviourDescUI.SetBehaviourInfo))]
        [HarmonyPostfix]
        public static void BattleDiceCard_BehaviourDescUI_SetBehaviourInfo(BattleDiceCard_BehaviourDescUI __instance, DiceBehaviour behaviour, LorId cardId, List<DiceBehaviour> behaviourList)
        {
            if(PassiveAbility_2160053.ParryDict.TryGetValue(cardId,out List<ParryStruct>  parry))
            {
                foreach(ParryStruct p in parry)
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
            string output= "";
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
        [HarmonyPatch(typeof(BattleDiceCardModel),nameof(BattleDiceCardModel.CreateDiceCardBehaviorList))]
        [HarmonyPostfix]
        public static void BattleDiceCardModel_CreateDiceCardBehaviorList(BattleDiceCardModel __instance, List<BattleDiceBehavior> __result)
        {
            if (__instance.owner!=null && __instance.owner.passiveDetail.HasPassive<PassiveAbility_2160053>())
            {
                if(PassiveAbility_2160053.ParryDict.TryGetValue(__instance.GetID(),out List<ParryStruct> parry))
                {
                    foreach(ParryStruct p in parry)
                    {
                        __result[p.index].AddAbility(new Parry(p.behaviour));
                    }
                }
            }
        }
        [HarmonyPatch(typeof(BattleCardAbilityDescXmlList),nameof(BattleCardAbilityDescXmlList.GetDefaultAbilityDescString))]
        [HarmonyPostfix]
        public static void BattleCardAbilityDescXmlList_GetDefaultAbilityDescString(DiceCardXmlInfo card, ref string __result)
        {
            if (card.id == Tools.MakeLorId(2160501))
                __result= string.Empty;
        }
        [HarmonyPatch(typeof(BattleFarAreaPlayManager),nameof(BattleFarAreaPlayManager.RollVictimsDice))]
        [HarmonyPostfix]
        public static void BattleFarAreaPlayManager_RollVictimsDice(BattleFarAreaPlayManager.VictimInfo v)
        {
            BattlePlayingCardDataInUnitModel aoe = BattleFarAreaPlayManager.Instance.attacker.currentDiceAction;
            if (aoe.card.GetID()== Tools.MakeLorId(2160501) && v.unitModel == aoe.target && v.playingCard!=null)
            {
                int sum = 0;
                foreach(BattleDiceBehavior dice in v.playingCard.GetDiceBehaviorList())
                {
                    if (dice != v.playingCard.currentBehavior)
                    {
                        dice.BeforeRollDice(null);
                        dice.RollDice();
                        dice.UpdateDiceFinalValue();
                        sum += dice.DiceResultValue;
                    }
                }
                if(v.playingCard.currentBehavior!=null)
                    v.playingCard.currentBehavior._diceFinalResultValue += sum;
            }
        }
        [HarmonyPatch(typeof(BattleUnitTargetArrowManagerUI),nameof(BattleUnitTargetArrowManagerUI.UpdateTargetListData))]
        [HarmonyPrefix]
        public static bool BattleUnitTargetArrowManagerUI_UpdateTargetListData(BattleUnitTargetArrowManagerUI __instance)
        {
            if(StageController.Instance._stageModel!=null && StageController.Instance._stageModel.ClassInfo.id == Tools.MakeLorId(21600053))
            {
                __instance.TargetListData.Clear();
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList();
                for (int i = 0; i < aliveList.Count; i++)
                {
                    BattleUnitModel battleUnitModel = aliveList[i];
                    List<BattleUnitTargetArrowData> collection = new List<BattleUnitTargetArrowData>();
                    List<BattlePlayingCardDataInUnitModel> cardAry = battleUnitModel.cardSlotDetail.cardAry;
                    for (int j = 0; j < cardAry.Count; ++j)
                    {
                        BattlePlayingCardDataInUnitModel cardDataInUnitModel = cardAry[j];
                        if (cardDataInUnitModel != null)
                        {
                            SpeedDiceUI ownerSpeedDice = battleUnitModel.view.speedDiceSetterUI.GetSpeedDiceByIndex(j);
                            if (ownerSpeedDice != null)
                            {
                                BattleUnitTargetArrowData data = new BattleUnitTargetArrowData();
                                data.faction = battleUnitModel.faction;
                                data.Dice = ownerSpeedDice;
                                SpeedDiceUI targetSpeedDice = cardDataInUnitModel.target.view.speedDiceSetterUI.GetSpeedDiceByIndex(cardDataInUnitModel.targetSlotOrder);
                                if (targetSpeedDice != null)
                                {
                                    data.TargetDice = targetSpeedDice;
                                    if (cardDataInUnitModel.card.GetID() == Tools.MakeLorId(2160501))
                                        data.isPairing = true;
                                    else if (targetSpeedDice.CardInDice != null && targetSpeedDice.CardInDice.target == battleUnitModel && targetSpeedDice.CardInDice.targetSlotOrder == j)
                                        data.isPairing = true;
                                }
                                List<BattleUnitTargetArrowData> all = collection.FindAll(x => x.TargetDice == data.TargetDice);
                                data.heightIdx = all == null ? 1 : all.Count + 1;
                                if (cardDataInUnitModel.subTargets != null && cardDataInUnitModel.subTargets.Count > 0)
                                {
                                    foreach (BattlePlayingCardDataInUnitModel.SubTarget subTarget in cardDataInUnitModel.subTargets)
                                    {
                                        Transform transform = battleUnitModel.view.speedDiceSetterUI.GetSpeedDiceByIndex(j).transform;
                                        BattleUnitModel target = subTarget.target;
                                        int targetSlotOrder = subTarget.targetSlotOrder;
                                        if (target.view.speedDiceSetterUI.GetSpeedDiceByIndex(targetSlotOrder) != null)
                                            data.SubTargetDices.Add(target.view.speedDiceSetterUI.GetSpeedDiceByIndex(targetSlotOrder));
                                    }
                                }
                                collection.Add(data);
                            }
                        }
                    }
                    __instance.TargetListData.AddRange(collection);
                }
                return false;
            }
            return true;
        }
    }
}
