﻿using System;
using HarmonyLib;
using System.Reflection;
using System.IO;
using UI;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using BaseMod;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using UnityEngine.Networking;
using GameSave;
using UnityEngine.UI;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    public class Harmony_Patch
    {
        public static bool BattleStoryPanel_Init;
        public static bool CenterPanel_Init;
        public static string ModPath;
        public static Dictionary<string, AudioClip> BGM;
        public static Dictionary<List<StageClassInfo>, UIStoryProgressIconSlot> Storyslots;
        public static List<BattlePlayingCardDataInUnitModel> FastCards;
        public Harmony_Patch()
        {
            try
            {
                Harmony harmony = new Harmony("Kazimier");
                ModPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
                BGM = new Dictionary<string, AudioClip>();
                FastCards = new List<BattlePlayingCardDataInUnitModel>();
                PatchGeneral(ref harmony);
                PatchStoryLine(ref harmony);
                //PatchEntryCG(ref harmony);
                GetBGMs(new DirectoryInfo(ModPath + "/BGM"));
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(20600003), ModPath + "/ArtWork/background.png");
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(21600013), ModPath + "/ArtWork/Candle.png");
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(21600023), ModPath + "/ArtWork/Street.png");
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(21600043), ModPath + "/ArtWork/Tournament.png");
            }
            catch(Exception ex)
            {
                File.WriteAllText(Application.dataPath + "/Mods/Kazimier.log", ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        public static void GetBGMs(DirectoryInfo dir)
        {
            if (dir.GetDirectories().Length != 0)
            {
                foreach (DirectoryInfo directory in dir.GetDirectories())
                    GetBGMs(directory);
            }
            foreach (FileInfo file in dir.GetFiles())
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(file.FullName);
                BGM[withoutExtension] = Tools.GetAudio(file.FullName);
            }
        }
        private void PatchGeneral(ref Harmony harmony)
        {
            MethodInfo Patch1 = typeof(Harmony_Patch).GetMethod("DropBookInventoryModel_GetBookList_invitationBookList");
            MethodInfo Method1 = typeof(DropBookInventoryModel).GetMethod("GetBookList_invitationBookList", AccessTools.all);
            harmony.Patch(Method1, postfix: new HarmonyMethod(Patch1));
/*            MethodInfo Patch2 = typeof(Harmony_Patch).GetMethod("UIInvitationDropBookSlot_SetData_DropBook");
            MethodInfo Method2 = typeof(UIInvitationDropBookSlot).GetMethod("SetData_DropBook", AccessTools.all);
            harmony.Patch(Method2, postfix: new HarmonyMethod(Patch2));*/
            MethodInfo Patch4 = typeof(Harmony_Patch).GetMethod("StageController_ActivateStartBattleEffectPhase");
            MethodInfo Method4 = typeof(StageController).GetMethod("ActivateStartBattleEffectPhase", AccessTools.all);
            harmony.Patch(Method4, prefix: new HarmonyMethod(Patch4));
            MethodInfo Patch5 = typeof(Harmony_Patch).GetMethod("StageController_MoveUnitPhase");
            MethodInfo Mehthod5 = typeof(StageController).GetMethod("MoveUnitPhase", AccessTools.all);
            harmony.Patch(Mehthod5, prefix: new HarmonyMethod(Patch5));
            MethodInfo Patch6 = typeof(Harmony_Patch).GetMethod("StageController_WaitUnitArrivePhase");
            MethodInfo Method6 = typeof(StageController).GetMethod("WaitUnitArrivePhase", AccessTools.all);
            harmony.Patch(Method6, prefix: new HarmonyMethod(Patch6));
        }
        private void PatchStoryLine(ref Harmony harmony)
        {
            MethodInfo method1 = typeof(Harmony_Patch).GetMethod("UIStoryProgressPanel_SetStoryLine");
            MethodInfo method2 = typeof(UIStoryProgressPanel).GetMethod("SetStoryLine", AccessTools.all);
            harmony.Patch(method2, postfix: new HarmonyMethod(method1));
        }
        private void PatchEntryCG(ref Harmony harmony)
        {
            MethodInfo method1 = typeof(LibraryModel).GetMethod("OnClearStage", AccessTools.all);
            harmony.Patch(method1, postfix: new HarmonyMethod(typeof(Harmony_Patch).GetMethod("LibraryModel_OnClearStage")));
            MethodInfo method2 = typeof(EntryScene).GetMethod("SetCG", AccessTools.all);
            harmony.Patch(method2, postfix: new HarmonyMethod(typeof(Harmony_Patch).GetMethod("EntryScene_SetCG")));
        }
        public static void DropBookInventoryModel_GetBookList_invitationBookList(ref List<LorId> __result)
        {
            __result.Add(Tools.MakeLorId(2060000));
            if (LibraryModel.Instance?.ClearInfo?.GetClearCount(Tools.MakeLorId(20600003)) >= 1)
                __result.Add(Tools.MakeLorId(2160000));
            if (!__result.Contains(Tools.MakeLorId(2160001)) && LibraryModel.Instance?.ClearInfo?.GetClearCount(Tools.MakeLorId(21600013)) >= 1)
                __result.Add(Tools.MakeLorId(2160001));
            if (!__result.Contains(Tools.MakeLorId(2160002)) && LibraryModel.Instance?.ClearInfo?.GetClearCount(Tools.MakeLorId(21600023))>=1)
                __result.Add(Tools.MakeLorId(2160002));
        }
        public static void UIInvitationDropBookSlot_SetData_DropBook(ref TextMeshProUGUI ___txt_bookNum, LorId bookId)
        {
            if (Singleton<DropBookInventoryModel>.Instance.GetBookCount(bookId) == 0)
                ___txt_bookNum.text = "∞";
        }
        public static void StageController_ActivateStartBattleEffectPhase(List<BattlePlayingCardDataInUnitModel> ____allCardList)
        {
            FastCards.Clear();
            PassiveAbility_2160127.owners.FindAll(x => x != null && x.cardSlotDetail?.cardQueue?.Count>0).ForEach(x => FastCards.Add(x.cardSlotDetail?.cardAry?.Find(x => x != null)));
            List<BattlePlayingCardDataInUnitModel> LateAttack = ____allCardList.FindAll(x => IsLateAttack(x));
            ____allCardList.RemoveAll(x => LateAttack.Contains(x));
            List<BattlePlayingCardDataInUnitModel> FastAttack = ____allCardList.FindAll(x => IsFastAttack(x));
            ____allCardList.RemoveAll(x => FastAttack.Contains(x));
            FastAttack.ForEach(x => ____allCardList.Insert(0, x));
            ____allCardList.AddRange(LateAttack);
        }
        public static bool StageController_MoveUnitPhase(ref StageController.StagePhase ____phase)
        {
            List<BattleUnitModel> battleUnitModelList1 = new List<BattleUnitModel>();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (alive.turnState == BattleUnitTurnState.BREAK || alive.currentDiceAction == null || alive.currentDiceAction.cardBehaviorQueue.Count == 0)
                    alive.moveDetail.Stop();
                else
                    battleUnitModelList1.Add(alive);
            }
            if (battleUnitModelList1.Count == 0)
                ____phase = StageController.StagePhase.RoundEndPhase;
            else
            {
                battleUnitModelList1.Sort((u1, u2) =>
                {
                    BattlePlayingCardDataInUnitModel currentDiceAction1 = u1.currentDiceAction;
                    BattlePlayingCardDataInUnitModel currentDiceAction2 = u2.currentDiceAction;
                    if (IsLateAttack(currentDiceAction1) || IsLateAttack(GetParry(currentDiceAction1)))
                    {
                        if (IsLateAttack(currentDiceAction2) || IsLateAttack(GetParry(currentDiceAction2)))
                        {
                            if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                                return 0;
                            return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                        }
                        else
                            return 1;
                    }
                    if (IsLateAttack(currentDiceAction2) || IsLateAttack(GetParry(currentDiceAction2)))
                    {
                        if (IsLateAttack(currentDiceAction1) || IsLateAttack(GetParry(currentDiceAction1)))
                        {
                            if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                                return 0;
                            return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                        }
                        else
                            return -1;
                    }
                    if (IsFastAttack(currentDiceAction1) || IsFastAttack(GetParry(currentDiceAction1)))
                    {
                        if (IsFastAttack(currentDiceAction2) || IsFastAttack(GetParry(currentDiceAction2)))
                        {
                            if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                                return 0;
                            return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                        }
                        else
                            return -1;
                    }
                    if (IsFastAttack(currentDiceAction2) || IsFastAttack(GetParry(currentDiceAction2)))
                    {
                        if (IsFastAttack(currentDiceAction1) || IsFastAttack(GetParry(currentDiceAction1)))
                        {
                            if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                                return 0;
                            return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                        }
                        else
                            return 1;
                    }
                    if (currentDiceAction1.card.GetSpec().Ranged == CardRange.Far)
                    {
                        if (currentDiceAction2.card.GetSpec().Ranged != CardRange.Far)
                            return -1;
                        if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                            return 0;
                        return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                    }
                    if (currentDiceAction2.card.GetSpec().Ranged == CardRange.Far)
                        return 1;
                    if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                        return 0;
                    return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                });
                int a = -1;
                for (int index = 0; index < battleUnitModelList1.Count; ++index)
                {
                    if (battleUnitModelList1[index].currentDiceAction.speedDiceResultValue > a)
                        a = battleUnitModelList1[index].currentDiceAction.speedDiceResultValue;
                }
                int num1 = Mathf.Min(a, 10);
                List<BattleUnitModel> battleUnitModelList2 = new List<BattleUnitModel>();
                List<BattleUnitModel> battleUnitModelList3 = new List<BattleUnitModel>();
                for (int index = 0; index < battleUnitModelList1.Count; ++index)
                {
                    BattleUnitModel unit = battleUnitModelList1[index];
                    BattleUnitModel target = unit.currentDiceAction.target;
                    if (target == null || target.IsDead() || target.IsExtinction())
                        unit.currentDiceAction = null;
                    else if (unit.turnState != BattleUnitTurnState.BREAK)
                    {
                        if (unit.currentDiceAction.card.GetSpec().Ranged == CardRange.Far && !IsLateAttack(unit.currentDiceAction.target?.currentDiceAction))
                        {
                            unit.moveDetail.Stop();
                            unit.currentDiceAction?.GetDiceBehaviorList();
                            battleUnitModelList2.Add(target);
                        }
                        else if (unit.currentDiceAction.card.GetSpec().Ranged == CardRange.Special)
                        {
                            unit.moveDetail.Stop();
                            battleUnitModelList3.Add(target);
                        }
                        else if (IsFastAttack(unit.currentDiceAction))
                        {
                            unit.moveDetail.Stop();
                            battleUnitModelList3.Add(target);
                        }
                        else
                        {
                            BattleDiceCardModel card = unit.currentDiceAction.target?.currentDiceAction?.card;
                            bool flag = false;
                            if (card != null && card.GetSpec().Ranged == CardRange.Far)
                            {
                                List<DiceBehaviour> behaviourList = card.GetBehaviourList();
                                if (behaviourList.Count > 0 && behaviourList[0].Type == BehaviourType.Atk)
                                    flag = true;
                            }
                            if (flag)
                                unit.moveDetail.Stop();
                            else if (battleUnitModelList2.Exists(x => x == unit))
                                unit.moveDetail.Stop();
                            else if (battleUnitModelList3.Exists(x => x == unit))
                                unit.moveDetail.Stop();
                            else
                            {
                                float num2 = 1f;
                                int num3 = Mathf.Min(unit.currentDiceAction.speedDiceResultValue, 10);
                                if (num3 < num1)
                                    num2 = (float)((double)num3 / (double)num1 * 0.5);
                                float speed = 60f * num2;
                                if ((double)Vector3.Distance(unit.view.WorldPosition, target.view.WorldPosition) <= (double)unit.moveDetail.GetDistanceBetweenDstAndTarget(unit, target) + 4.0)
                                {
                                    unit.moveDetail.Stop();
                                    target.moveDetail.Stop();
                                }
                                else
                                    unit.moveDetail.Move(target, speed);
                            }
                        }
                        unit.UpdateDirection(target.view.WorldPosition);
                    }
                    else
                    {
                        unit.currentDiceAction = null;
                        unit.moveDetail.Stop();
                    }
                }
                StageController.OnChangePhaseDelegate onChangePhase = Singleton<StageController>.Instance.onChangePhase;
                if (onChangePhase != null)
                    onChangePhase(____phase, StageController.StagePhase.WaitUnitsArrive);
                ____phase = StageController.StagePhase.WaitUnitsArrive;
                SingletonBehavior<BattleCamManager>.Instance.StartMoveUnits();
            }
            return false;
        }
        public static bool StageController_WaitUnitArrivePhase(ref StageController.StagePhase ____phase)
        {
            List<BattleUnitModel> battleUnitModelList1 = new List<BattleUnitModel>();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (alive.turnState == BattleUnitTurnState.BREAK || alive.currentDiceAction == null || alive.currentDiceAction.cardBehaviorQueue.Count == 0)
                    alive.moveDetail.Stop();
                else
                    battleUnitModelList1.Add(alive);
            }
            if (battleUnitModelList1.Count == 0)
                ____phase = StageController.StagePhase.RoundEndPhase;
            else
            {
                battleUnitModelList1.Sort((u1, u2) =>
                {
                    BattlePlayingCardDataInUnitModel currentDiceAction1 = u1.currentDiceAction;
                    BattlePlayingCardDataInUnitModel currentDiceAction2 = u2.currentDiceAction;
                    if (IsLateAttack(currentDiceAction1) || IsLateAttack(GetParry(currentDiceAction1)))
                    {
                        if (IsLateAttack(currentDiceAction2) || IsLateAttack(GetParry(currentDiceAction2)))
                        {
                            if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                                return 0;
                            return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                        }
                        else
                            return 1;
                    }
                    if (IsLateAttack(currentDiceAction2) || IsLateAttack(GetParry(currentDiceAction2)))
                    {
                        if (IsLateAttack(currentDiceAction1) || IsLateAttack(GetParry(currentDiceAction1)))
                        {
                            if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                                return 0;
                            return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                        }
                        else
                            return -1;
                    }
                    if (IsFastAttack(currentDiceAction1) || IsFastAttack(GetParry(currentDiceAction1)))
                    {
                        if (IsFastAttack(currentDiceAction2) || IsFastAttack(GetParry(currentDiceAction2)))
                        {
                            if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                                return 0;
                            return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                        }
                        else
                            return -1;
                    }
                    if (IsFastAttack(currentDiceAction2) || IsFastAttack(GetParry(currentDiceAction2)))
                    {
                        if (IsFastAttack(currentDiceAction1) || IsFastAttack(GetParry(currentDiceAction1)))
                        {
                            if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                                return 0;
                            return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                        }
                        else
                            return 1;
                    }
                    if (currentDiceAction1.card.GetSpec().Ranged == CardRange.Far)
                    {
                        if (currentDiceAction2.card.GetSpec().Ranged != CardRange.Far)
                            return -1;
                        if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                            return 0;
                        return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                    }
                    if (currentDiceAction2.card.GetSpec().Ranged == CardRange.Far)
                        return 1;
                    if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                        return 0;
                    return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                });
                int num1 = -1;
                List<BattleUnitModel> battleUnitModelList2 = new List<BattleUnitModel>();
                for (int index = 0; index < battleUnitModelList1.Count; ++index)
                {
                    if (battleUnitModelList1[index].currentDiceAction.speedDiceResultValue > num1)
                    {
                        num1 = battleUnitModelList1[index].currentDiceAction.speedDiceResultValue;
                        battleUnitModelList2.Add(battleUnitModelList1[index]);
                        List<BattleUnitModel> battleUnitModelList3 = new List<BattleUnitModel>();
                        foreach (BattleUnitModel battleUnitModel in battleUnitModelList2)
                        {
                            if (battleUnitModel.currentDiceAction.speedDiceResultValue < num1)
                                battleUnitModelList3.Add(battleUnitModel);
                        }
                        foreach (BattleUnitModel battleUnitModel in battleUnitModelList3)
                            battleUnitModelList2.Remove(battleUnitModel);
                    }
                    else if (battleUnitModelList1[index].currentDiceAction.speedDiceResultValue == num1 && num1 > -1)
                        battleUnitModelList2.Add(battleUnitModelList1[index]);
                }
                BattleUnitModel arrivedUnit = null;
                for (int index = 0; index < battleUnitModelList1.Count; ++index)
                {
                    BattleUnitModel unit = battleUnitModelList1[index];
                    BattleUnitModel target = unit.currentDiceAction.target;
                    if (target != null)
                    {
                        if (unit.moveDetail.isArrived && !unit.moveDetail.isKnockback)
                        {
                            if (battleUnitModelList2.Exists(x => x == unit))
                            {
                                arrivedUnit = unit;
                                break;
                            }
                            if (unit.currentDiceAction.card.GetSpec().Ranged == CardRange.Special)
                            {
                                arrivedUnit = unit;
                                break;
                            }
                            if (IsFastAttack(unit.currentDiceAction))
                            {
                                arrivedUnit = unit;
                                break;
                            }
                            if (unit.currentDiceAction.card.GetSpec().Ranged == CardRange.Far && !IsLateAttack(unit.currentDiceAction.target.currentDiceAction!))
                            {
                                arrivedUnit = unit;
                                break;
                            }
                        }
                        else if (!unit.moveDetail.isKnockback)
                        {
                            float num2 = 1f;
                            int num3 = Mathf.Min(unit.currentDiceAction.speedDiceResultValue, 10);
                            if (num3 < num1)
                                num2 = (float)((double)num3 / (double)num1 * 0.5);
                            if ((double)Vector3.Distance(unit.view.WorldPosition, target.view.WorldPosition) <= (double)unit.moveDetail.GetDistanceBetweenDstAndTarget(unit, target) + 4.0)
                            {
                                unit.moveDetail.Stop();
                                target.moveDetail.Stop();
                            }
                        }
                    }
                }
                if (arrivedUnit == null)
                    return false;
                for (int index = 0; index < battleUnitModelList1.Count; ++index)
                {
                    if (battleUnitModelList1[index] != arrivedUnit && !battleUnitModelList1[index].moveDetail.isKnockback)
                        battleUnitModelList1[index].moveDetail.MoveSlow(true);
                }
                int slotOrder = arrivedUnit.currentDiceAction.slotOrder;
                BattleUnitModel target1 = arrivedUnit.currentDiceAction.target;
                int targetSlotOrder = arrivedUnit.currentDiceAction.targetSlotOrder;
                BattlePlayingCardDataInUnitModel cardB = null;
                bool flag = false;
                try
                {
                    cardB = target1.cardSlotDetail.cardAry[targetSlotOrder];
                    if (cardB != null)
                    {
                        int? count = cardB.cardBehaviorQueue?.Count;
                        int num2 = 0;
                        if (count.GetValueOrDefault() > num2 & count.HasValue)
                        {
                            if (cardB.isDestroyed)
                                flag = false;
                            else if (cardB.card.GetSpec().affection == CardAffection.TeamNear)
                            {
                                BattlePlayingCardDataInUnitModel.SubTarget subTarget = cardB.subTargets.Find(x => x.target == arrivedUnit && x.targetSlotOrder == slotOrder);
                                if (subTarget != null)
                                {
                                    cardB.subTargets.Add(new BattlePlayingCardDataInUnitModel.SubTarget()
                                    {
                                        target = cardB.target,
                                        targetSlotOrder = cardB.targetSlotOrder
                                    });
                                    cardB.target = subTarget.target;
                                    cardB.targetSlotOrder = subTarget.targetSlotOrder;
                                    cardB.subTargets.Remove(subTarget);
                                }
                                if (cardB.target == arrivedUnit)
                                {
                                    if (cardB.targetSlotOrder == slotOrder)
                                    {
                                        if (!arrivedUnit.IsBreakLifeZero())
                                        {
                                            if (!target1.IsBreakLifeZero())
                                                flag = true;
                                        }
                                    }
                                }
                            }
                            else if (cardB.target == arrivedUnit)
                            {
                                if (cardB.targetSlotOrder == slotOrder)
                                {
                                    if (!arrivedUnit.IsBreakLifeZero())
                                    {
                                        if (!target1.IsBreakLifeZero())
                                            flag = true;
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
                if (flag)
                {
                    target1.moveDetail.MoveSlow(false);
                    for (int index = 0; index < BattleObjectManager.instance.GetList().Count; ++index)
                    {
                        BattleUnitModel battleUnitModel = BattleObjectManager.instance.GetList()[index];
                        if (battleUnitModel != target1 && battleUnitModel != arrivedUnit && !battleUnitModel.IsDead())
                            battleUnitModel.view.charAppearance.ChangeColor(Color.grey);
                    }
                    Singleton<StageController>.Instance.sp(arrivedUnit.currentDiceAction, cardB);
                }
                else if (target1.cardSlotDetail.keepCard.cardBehaviorQueue.Count > 0)
                {
                    target1.moveDetail.MoveSlow(false);
                    for (int index = 0; index < BattleObjectManager.instance.GetList().Count; ++index)
                    {
                        BattleUnitModel battleUnitModel = BattleObjectManager.instance.GetList()[index];
                        if (battleUnitModel != target1 && battleUnitModel != arrivedUnit && !battleUnitModel.IsDead())
                            battleUnitModel.view.charAppearance.ChangeColor(Color.grey);
                    }
                    target1.cardSlotDetail.keepCard.target = arrivedUnit;
                    BattleDiceCardModel card = arrivedUnit.currentDiceAction.card;
                    if ((card != null ? (card.GetSpec().Ranged == CardRange.Far ? 1 : 0) : 0) != 0)
                        target1.moveDetail.Stop();
                    else
                        target1.moveDetail.Move(arrivedUnit, 15f);
                    Singleton<StageController>.Instance.sp(arrivedUnit.currentDiceAction, target1.cardSlotDetail.keepCard);
                }
                else
                {
                    if (arrivedUnit.currentDiceAction.target != null)
                        arrivedUnit.currentDiceAction.target.moveDetail.Stop();
                    for (int index = 0; index < BattleObjectManager.instance.GetList().Count; ++index)
                    {
                        BattleUnitModel battleUnitModel = BattleObjectManager.instance.GetList()[index];
                        if (battleUnitModel != target1 && battleUnitModel != arrivedUnit && !battleUnitModel.IsDead())
                            battleUnitModel.view.charAppearance.ChangeColor(Color.grey);
                    }
                    MethodInfo Action = typeof(StageController).GetMethod("StartAction", AccessTools.all);
                    Action.Invoke(Singleton<StageController>.Instance, new object[] { arrivedUnit.currentDiceAction });
                }
            }
            return false;
        }
        public static void UIStoryProgressPanel_SetStoryLine(UIStoryProgressPanel __instance)
        {
            if (__instance.gameObject.transform.parent.gameObject.name== "[Rect]CenterPanel" && !CenterPanel_Init)
            {
                if (Storyslots == null)
                    Storyslots = new Dictionary<List<StageClassInfo>, UIStoryProgressIconSlot>();
                CreateStoryLine(__instance, 20600003, UIStoryLine.CaneOffice, new Vector3(500f, 0.0f));
                CreateStoryLine(__instance, 21600013, UIStoryLine.CaneOffice, new Vector3(250f, 160f));
                CreateStoryLine(__instance, 21600023, UIStoryLine.CaneOffice, new Vector3(375f, 320f));
                //CreateStoryLine(__instance, 21600033, UIStoryLine.CaneOffice, new Vector3(625f, 320f));
                CreateStoryLine(__instance, 21600043, UIStoryLine.CaneOffice, new Vector3(750f, 160f));
                //CreateStoryLine(__instance, 21600053, UIStoryLine.CaneOffice, new Vector3(500f, 480f));
                CenterPanel_Init = true;
            }
            if (__instance.gameObject.transform.parent.gameObject.name == "BattleStoryPanel" && !BattleStoryPanel_Init)
            {
                if(Storyslots==null)
                    Storyslots = new Dictionary<List<StageClassInfo>, UIStoryProgressIconSlot>();
                CreateStoryLine(__instance, 20600003, UIStoryLine.CaneOffice, new Vector3(500f, 0.0f));
                CreateStoryLine(__instance, 21600013, UIStoryLine.CaneOffice, new Vector3(250f, 160f));
                CreateStoryLine(__instance, 21600023, UIStoryLine.CaneOffice, new Vector3(375f, 320f));
                //CreateStoryLine(__instance, 21600033, UIStoryLine.CaneOffice, new Vector3(625f, 320f));
                CreateStoryLine(__instance, 21600043, UIStoryLine.CaneOffice, new Vector3(750f, 160f));
                //CreateStoryLine(__instance, 21600053, UIStoryLine.CaneOffice, new Vector3(500f, 480f));
                BattleStoryPanel_Init = true;
            }
            foreach (List<StageClassInfo> key in Storyslots.Keys)
            {
                Storyslots[key].SetSlotData(key);
                if (key[0].currentState != StoryState.Close)
                    Storyslots[key].SetActiveStory(true);
                else
                    Storyslots[key].SetActiveStory(false);
            }
        }
        public static void CreateStoryLine(UIStoryProgressPanel __instance,int stageId, UIStoryLine reference, Vector3 vector)
        {
            StageClassInfo data = Singleton<StageClassInfoList>.Instance.GetData(Tools.MakeLorId(stageId));
            if (Storyslots.TryGetValue(new List<StageClassInfo>() { data }, out UIStoryProgressIconSlot slot))
            {
                slot.Initialized(__instance);
                return;
            }
            UIStoryProgressIconSlot progressIconSlot = ((List<UIStoryProgressIconSlot>)typeof(UIStoryProgressPanel).GetField("iconList", AccessTools.all).GetValue((object)__instance)).Find(x => x.currentStory == reference);
            UIStoryProgressIconSlot newslot = UnityEngine.Object.Instantiate<UIStoryProgressIconSlot>(progressIconSlot, progressIconSlot.transform.parent);
            newslot.currentStory = UIStoryLine.Rats;
            newslot.Initialized(__instance);
            newslot.transform.localPosition += vector;
            typeof(UIStoryProgressIconSlot).GetField("connectLineList", AccessTools.all).SetValue(newslot, new List<GameObject>());
            Storyslots[new List<StageClassInfo>() { data }] = newslot;
        }
        private static void LibraryModel_OnClearStage(LorId stageId)
        {
            if (stageId == Tools.MakeLorId(20600003))
            {
                LatestDataModel data = new LatestDataModel();
                Singleton<SaveManager>.Instance.LoadLatestData(data);
                data.LatestStorychapter = 9;
                data.LatestStorygroup = 20600003;
                data.LatestStoryepisode = 20600003;
                data.RuinTitle = LibraryModel.Instance.PlayHistory.Clear_EndcontentsAllStage == 1 ? 1 : 0;
                Singleton<SaveManager>.Instance.SaveLatestData(data);
            }
        }
        private static void EntryScene_SetCG(EntryScene __instance,LatestDataModel ____latestData)
        {
            if (____latestData.LatestStorychapter==6 &&  ____latestData.LatestStorygroup== 20600003 && ____latestData.LatestStoryepisode== 20600003)
            {
                Texture2D texture2D = new Texture2D(1, 1);
                texture2D.LoadImage(File.ReadAllBytes(Harmony_Patch.ModPath + "/ArtWork/background.png"));
                Sprite sprite= Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
                __instance.CGImage.sprite = sprite;
            }
        }
        public static BattlePlayingCardDataInUnitModel GetParry(BattlePlayingCardDataInUnitModel card)
        {
            try
            {
                BattlePlayingCardDataInUnitModel oppoist = card.target.cardSlotDetail.cardAry[card.targetSlotOrder];
                if (oppoist.owner.DirectAttack() || card.owner.DirectAttack())
                    return null;
                if (oppoist.target == card.owner && oppoist.targetSlotOrder == card.slotOrder)
                    return oppoist;
                else
                    return null;
            }
            catch
            {

            }
            return null;
        }
        public static bool IsLateAttack(BattlePlayingCardDataInUnitModel card)
        {
            if (card == null)
                return false;
            return card.cardAbility is DiceCardSelfAbility_LateAttack;
        }
        public static bool IsFastAttack(BattlePlayingCardDataInUnitModel card)
        {
            if (card == null)
                return false;
            if (FastCards.Contains(card))
                return true;
            if (card.cardAbility is DiceCardSelfAbility_OneSideFastAttack && GetParry(card) == null)
                return true;
            return card.cardAbility is DiceCardSelfAbility_FastAttack;
        }
        public static void UpdateInfo(BattleUnitModel unit) => SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(unit, unit.faction, unit.hp, unit.breakDetail.breakGauge);
        public static void AddNewCard(BattleUnitModel unit, List<int> cards, Queue<int> priority)
        {
            do
            {
                int p = 0;
                if (priority.Count > 0)
                    p = priority.Dequeue();
                unit.allyCardDetail.AddNewCard(Tools.MakeLorId(cards[0])).SetPriorityAdder(p);
                cards.RemoveAt(0);
            }
            while (cards.Count > 0);
        }
    }

}
